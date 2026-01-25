using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoodNest.Common;
using MoodNest.Data;
using MoodNest.Entities;
using MoodNest.Components.Model;
using System.Text.RegularExpressions;

namespace MoodNest.Components.Services;

public class JournalService : IJournalService
{
    private readonly AppDbContext _context;
    private readonly PinAuthService _auth;

    public JournalService(AppDbContext context, PinAuthService auth)
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

    /* =======================
       CREATE
    ======================== */

    public async Task<ServiceResult<bool>> CreateAsync(JournalEntryViewModel model)
    {
        try
        {
            int userId = RequireUser();

            var entity = new JournalEntry
            {
                UserId = userId,

                Title = model.Title,
                ContentHtml = model.ContentHtml,
                PrimaryMood = model.PrimaryMood,
                SecondaryMoods = model.SecondaryMoods,
                Category = model.Category,
                Tags = model.Tags,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.JournalEntries.Add(entity);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.FailureResult(ex.Message);
        }
    }

    /* =======================
       UPDATE
    ======================== */

    public async Task<ServiceResult<bool>> UpdateAsync(int id, JournalEntryViewModel model)
    {
        try
        {
            int userId = RequireUser();

            var entity = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entity == null)
                return ServiceResult<bool>.FailureResult("Journal entry not found");

            entity.Title = model.Title;
            entity.ContentHtml = model.ContentHtml;
            entity.PrimaryMood = model.PrimaryMood;
            entity.SecondaryMoods = model.SecondaryMoods;
            entity.Category = model.Category;
            entity.Tags = model.Tags;
            entity.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.FailureResult(ex.Message);
        }
    }

    /* =======================
       GET BY ID
    ======================== */

    public async Task<ServiceResult<JournalEntryViewModel>> GetByIdAsync(int id)
    {
        try
        {
            int userId = RequireUser();

            var entity = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entity == null)
                return ServiceResult<JournalEntryViewModel>
                    .FailureResult("Journal entry not found");

            var model = new JournalEntryViewModel
            {
                Title = entity.Title,
                ContentHtml = entity.ContentHtml,
                PrimaryMood = entity.PrimaryMood,
                SecondaryMoods = entity.SecondaryMoods,
                Category = entity.Category,
                Tags = entity.Tags,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return ServiceResult<JournalEntryViewModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            return ServiceResult<JournalEntryViewModel>.FailureResult(ex.Message);
        }
    }

    /* =======================
       DELETE
    ======================== */

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            int userId = RequireUser();

            var entity = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entity == null)
                return ServiceResult<bool>.FailureResult("Journal entry not found");

            _context.JournalEntries.Remove(entity);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.FailureResult(ex.Message);
        }
    }

    /* =======================
       GET ALL
    ======================== */

    public async Task<ServiceResult<List<JournalEntryDisplayModel>>> GetAllAsync()
    {
        try
        {
            int userId = RequireUser();

            var entries = await _context.JournalEntries
                .Where(e => e.UserId == userId)
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
                .SuccessResult(entries);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<JournalEntryDisplayModel>>
                .FailureResult(ex.Message);
        }
    }

    /* =======================
       TODAY
    ======================== */

    public async Task<ServiceResult<JournalEntryDisplayModel?>> GetTodayAsync()
    {
        try
        {
            int userId = RequireUser();
            var today = DateTime.Today;

            var entry = await _context.JournalEntries
                .Where(e => e.UserId == userId && e.CreatedAt.Date == today)
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
                .FirstOrDefaultAsync();

            return ServiceResult<JournalEntryDisplayModel?>.SuccessResult(entry);
        }
        catch (Exception ex)
        {
            return ServiceResult<JournalEntryDisplayModel?>.FailureResult(ex.Message);
        }
    }

    /* =======================
       STATS / ANALYTICS
    ======================== */

    public async Task<ServiceResult<JournalStatsModel>> GetJournalStatsAsync()
    {
        try
        {
            int userId = RequireUser();

            var entries = await _context.JournalEntries
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.CreatedAt)
                .ToListAsync();

            if (!entries.Any())
                return ServiceResult<JournalStatsModel>.SuccessResult(new JournalStatsModel());

            var dates = entries
                .Select(e => e.CreatedAt.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            int longest = 1;
            int temp = 1;
            int current = 0;

            for (int i = 1; i < dates.Count; i++)
            {
                if ((dates[i] - dates[i - 1]).Days == 1)
                {
                    temp++;
                    longest = Math.Max(longest, temp);
                }
                else temp = 1;
            }

            if (dates.Last() == DateTime.Today)
                current = temp;

            int totalDays = (DateTime.Today - dates.First()).Days + 1;
            int missed = totalDays - dates.Count;

            string NormalizeMood(string mood)
            {
                if (string.IsNullOrWhiteSpace(mood)) return mood;
                var parts = mood.Split(' ');
                return parts.Length > 1 ? parts[1] : parts[0];
            }

            var moods = entries.Select(e => NormalizeMood(e.PrimaryMood)).ToList();

            var moodCounts = moods
                .GroupBy(m => m)
                .OrderByDescending(g => g.Count())
                .ToList();

            int positive = moods.Count(m => new[] { "Happy", "Excited", "Relaxed", "Grateful", "Confident" }.Contains(m));
            int neutral  = moods.Count(m => new[] { "Calm", "Thoughtful", "Curious", "Nostalgic", "Bored" }.Contains(m));
            int negative = moods.Count - positive - neutral;

            int total = moods.Count;

            return ServiceResult<JournalStatsModel>.SuccessResult(
                new JournalStatsModel
                {
                    CurrentStreak = current,
                    LongestStreak = longest,
                    MissedDays = Math.Max(0, missed),
                    MostFrequentMood = moodCounts.First().Key,
                    PositivePercentage = (int)Math.Round((double)positive / total * 100),
                    NeutralPercentage  = (int)Math.Round((double)neutral  / total * 100),
                    NegativePercentage = (int)Math.Round((double)negative / total * 100)
                }
            );
        }
        catch (Exception ex)
        {
            return ServiceResult<JournalStatsModel>.FailureResult(ex.Message);
        }
    }

    /* =======================
       TAGS / WORDS / EXPORT
    ======================== */

    public async Task<List<TagStatModel>> GetTagStatsAsync()
    {
        int userId = RequireUser();

        var entries = await _context.JournalEntries
            .Where(e => e.UserId == userId)
            .ToListAsync();

        return entries
            .Where(e => !string.IsNullOrWhiteSpace(e.Tags))
            .SelectMany(e => e.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Select(t => t.Trim())
            .GroupBy(t => t)
            .Select(g => new TagStatModel { Tag = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();
    }

    public async Task<List<WordTrendModel>> GetWordTrendsAsync()
    {
        int userId = RequireUser();

        var entries = await _context.JournalEntries
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

        return entries.Select(e => new WordTrendModel
        {
            Date = e.CreatedAt.Date,
            WordCount = string.IsNullOrWhiteSpace(e.ContentHtml)
                ? 0
                : StripHtml(e.ContentHtml).Split(' ', StringSplitOptions.RemoveEmptyEntries).Length
        }).ToList();
    }

    private static string StripHtml(string html) =>
        Regex.Replace(html ?? "", "<.*?>", string.Empty);

    public async Task<List<JournalEntry>> GetEntriesBetweenAsync(DateTime from, DateTime to)
    {
        int userId = RequireUser();

        return await _context.JournalEntries
            .Where(e => e.UserId == userId &&
                        e.CreatedAt.Date >= from.Date &&
                        e.CreatedAt.Date <= to.Date)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }
}
