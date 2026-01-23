using System.Collections.Generic;
using System.Threading.Tasks;
using MoodNest.Common;
using MoodNest.Components.Model;

namespace MoodNest.Components.Services;

public interface IJournalFilterService
{
    Task<ServiceResult<List<JournalEntryDisplayModel>>>
        GetFilteredAsync(JournalFilterModel filter);
}