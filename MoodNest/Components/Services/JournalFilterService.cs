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

    public JournalFilterService(AppDbContext context)
        => _context = context;

    public async Task<ServiceResult<List<JournalEntryDisplayModel>>>
        GetFilteredAsync(JournalFilterModel filter)
    {
        try
        {
            var query = _context.JournalEntries.AsQueryable();

            /* =======================
               SEARCH (TITLE + CONTENT)
            ======================== */

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var text = filter.SearchText.ToLower();

                query = query.Where(e =>
                    e.Title.ToLower().Contains(text) ||
                    e.ContentHtml.ToLower().Contains(text)
                );
            }

            /* =======================
               DATE RANGE
            ======================== */

            if (filter.FromDate.HasValue)
                query = query.Where(e => e.CreatedAt >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(e => e.CreatedAt <= filter.ToDate.Value);

            /* =======================
               MOODS (PRIMARY + SECONDARY)
            ======================== */

            if (filter.Moods.Any())
            {
                query = query.Where(e =>
                    filter.Moods.Any(m =>
                        e.PrimaryMood.Contains(m) ||
                        (e.SecondaryMoods != null &&
                         e.SecondaryMoods.Contains(m))
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
                        e.Tags != null &&
                        e.Tags.Contains(t)
                    )
                );
            }

            /* =======================
               PROJECTION (LIST PAGE)
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
