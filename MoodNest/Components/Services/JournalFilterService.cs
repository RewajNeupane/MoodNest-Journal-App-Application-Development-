using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoodNest.Common;
using MoodNest.Data;
using MoodNest.Entities;
using MoodNest.Components.Model;

namespace MoodNest.Components.Services;

public class JournalFilterService : IJournalFilterService
{
    private readonly AppDbContext _context;
    private readonly PinAuthService _auth;

    public JournalFilterService(AppDbContext context, PinAuthService auth)
    {
        _context = context;
        _auth = auth;
    }

    /* =======================
       AUTH GUARD
    ======================== */

    private int RequireUser()
    {
        if (_auth.CurrentUserId == null)
            throw new InvalidOperationException("User not authenticated");

        return _auth.CurrentUserId.Value;
    }

    public async Task<ServiceResult<List<JournalEntryDisplayModel>>>
        GetFilteredAsync(JournalFilterModel filter)
    {
        try
        {
            int userId = RequireUser();

            // 🔐 USER-SCOPED QUERY (CRITICAL FIX)
            var query = _context.JournalEntries
                .Where(e => e.UserId == userId)
                .AsQueryable();

            /* =======================
               SEARCH (TITLE + CONTENT)
            ======================== */

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var text = filter.SearchText.ToLower();

                query = query.Where(e =>
                    (e.Title != null && e.Title.ToLower().Contains(text)) ||
                    (e.ContentHtml != null && e.ContentHtml.ToLower().Contains(text))
                );
            }

            /* =======================
               DATE RANGE
            ======================== */

            if (filter.FromDate.HasValue)
                query = query.Where(e => e.CreatedAt.Date >= filter.FromDate.Value.Date);

            if (filter.ToDate.HasValue)
                query = query.Where(e => e.CreatedAt.Date <= filter.ToDate.Value.Date);

            /* =======================
               CATEGORY
            ======================== */

            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                query = query.Where(e => e.Category == filter.Category);
            }

            /* =======================
               MOODS (PRIMARY + SECONDARY)
            ======================== */

            if (filter.Moods.Any())
            {
                query = query.Where(e =>
                    filter.Moods.Any(m =>
                        (e.PrimaryMood != null && e.PrimaryMood.Contains(m)) ||
                        (e.SecondaryMoods != null && e.SecondaryMoods.Contains(m))
                    )
                );
            }

            /* =======================
               TAGS
            ======================== */

            if (filter.Tags.Any())
            {
                query = query.Where(e =>
                    filter.Tags.Any(t =>
                        e.Tags != null && e.Tags.Contains(t)
                    )
                );
            }

            /* =======================
               PROJECTION
            ======================== */

            var result = await query
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new JournalEntryDisplayModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    PrimaryMood = e.PrimaryMood,
                    SecondaryMoods = e.SecondaryMoods,
                    Category = e.Category,
                    Tags = e.Tags,
                    Day = e.CreatedAt.Day,
                    Month = e.CreatedAt.ToString("MMMM"),
                    Year = e.CreatedAt.Year
                })
                .ToListAsync();

            return ServiceResult<List<JournalEntryDisplayModel>>
                .SuccessResult(result);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<JournalEntryDisplayModel>>
                .FailureResult(ex.Message);
        }
    }
}
