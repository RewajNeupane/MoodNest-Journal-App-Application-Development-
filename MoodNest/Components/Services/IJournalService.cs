using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoodNest.Common;
using MoodNest.Components.Model;
using MoodNest.Entities;

public interface IJournalService
{
    Task<ServiceResult<bool>> CreateAsync(JournalEntryViewModel model);
    Task<ServiceResult<bool>> UpdateAsync(int id, JournalEntryViewModel model);
    Task<ServiceResult<JournalEntryViewModel>> GetByIdAsync(int id);
    Task<ServiceResult<bool>> DeleteAsync(int id);
    Task<ServiceResult<List<JournalEntryDisplayModel>>> GetAllAsync();

    // NEW — used to enforce one entry per day
    Task<ServiceResult<JournalEntryDisplayModel?>> GetTodayAsync();
    
    Task<ServiceResult<JournalStatsModel>> GetJournalStatsAsync();
    Task<List<TagStatModel>> GetTagStatsAsync();
    Task<List<WordTrendModel>> GetWordTrendsAsync();
    

    Task<List<JournalEntry>> GetEntriesBetweenAsync(DateTime from, DateTime to);

    Task<List<JournalEntryDisplayModel>> GetPublicJournalsAsync();
    Task<ServiceResult<JournalEntryViewModel>> GetPublicByIdAsync(int id);



}