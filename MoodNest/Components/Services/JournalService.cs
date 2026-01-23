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

}
