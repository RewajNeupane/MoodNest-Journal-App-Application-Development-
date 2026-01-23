using System.Collections.Generic;
using System.Threading.Tasks;
using MoodNest.Common;
using MoodNest.Components.Model;

namespace MoodNest.Components.Services;

public interface IJournalService
{
    Task<ServiceResult<bool>> CreateAsync(JournalEntryViewModel model);

    Task<ServiceResult<bool>> UpdateAsync(int id, JournalEntryViewModel model);

    Task<ServiceResult<JournalEntryViewModel>> GetByIdAsync(int id);

    Task<ServiceResult<bool>> DeleteAsync(int id);

    Task<ServiceResult<List<JournalEntryDisplayModel>>> GetAllAsync();
}