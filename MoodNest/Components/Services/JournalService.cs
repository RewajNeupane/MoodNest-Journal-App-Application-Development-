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

    // Expression-bodied constructor (PDF style)
    public JournalService(AppDbContext context) => _context = context;

    /* =======================
       CREATE
    ======================== */

    public async Task<ServiceResult<bool>> CreateAsync(JournalEntryViewModel model)
    {
        try
        {
            var entity = new JournalEntry
            {
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
            var entity = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id);

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
            var entity = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id);

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
            var entity = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id);

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
       GET ALL (LIST PAGE)
    ======================== */

/* =======================
   GET ALL (LIST PAGE)
======================== */

    public async Task<ServiceResult<List<JournalEntryDisplayModel>>> GetAllAsync()
    {
        try
        {
            var entries = await _context.JournalEntries
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new JournalEntryDisplayModel
                {
                    Id = e.Id,
                    Title = e.Title,

                    // Moods & metadata
                    PrimaryMood = e.PrimaryMood,
                    SecondaryMoods = e.SecondaryMoods,
                    Category = e.Category,
                    Tags = e.Tags,

                    // Date parts (NO string slicing in UI)
                    Day = e.CreatedAt.Day,
                    Month = e.CreatedAt.ToString("MMMM"), // January, February, etc.
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
    public async Task<ServiceResult<JournalEntryDisplayModel?>> GetTodayAsync()
    {
        try
        {
            var today = DateTime.Today;

            var entry = await _context.JournalEntries
                .Where(e => e.CreatedAt.Date == today)
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
    public async Task<ServiceResult<JournalStatsModel>> GetJournalStatsAsync()
{
    try
    {
        var entries = await _context.JournalEntries
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

        if (!entries.Any())
        {
            return ServiceResult<JournalStatsModel>.SuccessResult(
                new JournalStatsModel()
            );
        }

        /* =======================
           STREAK LOGIC (UNCHANGED)
        ======================== */

        var dates = entries
            .Select(e => e.CreatedAt.Date)
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        int longest = 1;
        int current = 0;
        int temp = 1;

        for (int i = 1; i < dates.Count; i++)
        {
            if ((dates[i] - dates[i - 1]).Days == 1)
            {
                temp++;
                longest = Math.Max(longest, temp);
            }
            else
            {
                temp = 1;
            }
        }

        if (dates.Last() == DateTime.Today)
            current = temp;

        int totalDays =
            (DateTime.Today - dates.First()).Days + 1;

        int missed = totalDays - dates.Count;

        /* =======================
           MOOD ANALYTICS (FIXED)
        ======================== */

        // Normalize mood (remove emoji if present)
        string NormalizeMood(string mood)
        {
            if (string.IsNullOrWhiteSpace(mood))
                return mood;

            var parts = mood.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 ? parts[1] : parts[0];
        }

        var normalizedMoods = entries
            .Select(e => NormalizeMood(e.PrimaryMood))
            .ToList();

        // Most frequent mood
        var moodCounts = normalizedMoods
            .GroupBy(m => m)
            .Select(g => new
            {
                Mood = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .ToList();

        string? mostFrequentMood = moodCounts.First().Mood;

        // Mood breakdown
        int positive = 0;
        int neutral = 0;
        int negative = 0;

        foreach (var mood in normalizedMoods)
        {
            switch (mood)
            {
                case "Happy":
                case "Excited":
                case "Relaxed":
                case "Grateful":
                case "Confident":
                    positive++;
                    break;

                case "Calm":
                case "Thoughtful":
                case "Curious":
                case "Nostalgic":
                case "Bored":
                    neutral++;
                    break;

                default:
                    negative++;
                    break;
            }
        }

        int total = normalizedMoods.Count;

        return ServiceResult<JournalStatsModel>.SuccessResult(
            new JournalStatsModel
            {
                CurrentStreak = current,
                LongestStreak = longest,
                MissedDays = Math.Max(0, missed),

                MostFrequentMood = mostFrequentMood,

                PositivePercentage = (int)Math.Round((double)positive / total * 100),
                NeutralPercentage  = (int)Math.Round((double)neutral  / total * 100),
                NegativePercentage = (int)Math.Round((double)negative / total * 100)
            }
        );
    }
    catch (Exception ex)
    {
        return ServiceResult<JournalStatsModel>
            .FailureResult(ex.Message);
    }
}
    public async Task<List<TagStatModel>> GetTagStatsAsync()
    {
        var entries = await _context.JournalEntries.ToListAsync();

        return entries
            .Where(e => !string.IsNullOrWhiteSpace(e.Tags))
            .SelectMany(e => e.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Select(t => t.Trim())
            .GroupBy(t => t)
            .Select(g => new TagStatModel
            {
                Tag = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .ToList();
    }

    public async Task<List<WordTrendModel>> GetWordTrendsAsync()
    {
        var entries = await _context.JournalEntries
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

        return entries
            .Select(e => new WordTrendModel
            {
                Date = e.CreatedAt.Date,
                WordCount = string.IsNullOrWhiteSpace(e.ContentHtml)
                    ? 0
                    : StripHtml(e.ContentHtml).Split(' ', StringSplitOptions.RemoveEmptyEntries).Length
            })
            .ToList();
    }

    

    

    private static string StripHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        return Regex.Replace(html, "<.*?>", string.Empty);
    }
    
    public async Task<List<JournalEntry>> GetEntriesBetweenAsync(
        DateTime from,
        DateTime to)
    {
        return await _context.JournalEntries
            .Where(e => e.CreatedAt.Date >= from.Date &&
                        e.CreatedAt.Date <= to.Date)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }



    
    

    
}
