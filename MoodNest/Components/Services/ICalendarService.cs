using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICalendarService
{
    Task<List<CalendarDayModel>> GetMonthAsync(int year, int month);
    Task<bool> HasTodayEntryAsync();
    Task<List<DateTime>> GetMissedDaysAsync(int year, int month);
}