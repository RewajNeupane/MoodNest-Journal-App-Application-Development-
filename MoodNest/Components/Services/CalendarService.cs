using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoodNest.Data;
using MoodNest.Components.Services;

public class CalendarService : ICalendarService
{
    private readonly AppDbContext _context;
    private readonly PinAuthService _auth;

    public CalendarService(AppDbContext context, PinAuthService auth)
    {
        _context = context;
        _auth = auth;
    }

    private int RequireUser()
    {
        if (_auth.CurrentUserId == null)
            throw new InvalidOperationException("User not authenticated");

        return _auth.CurrentUserId.Value;
    }

    public async Task<List<CalendarDayModel>> GetMonthAsync(int year, int month)
    {
        int userId = RequireUser();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        var entries = await _context.JournalEntries
            .Where(e =>
                e.UserId == userId &&
                e.CreatedAt >= start &&
                e.CreatedAt < end)
            .ToListAsync();

        var daysInMonth = DateTime.DaysInMonth(year, month);
        var today = DateTime.Today;

        var result = new List<CalendarDayModel>();

        for (int d = 1; d <= daysInMonth; d++)
        {
            var date = new DateTime(year, month, d);
            var entry = entries.FirstOrDefault(e => e.CreatedAt.Date == date);

            result.Add(new CalendarDayModel
            {
                Date = date,
                IsToday = date.Date == today,
                HasEntry = entry != null,
                JournalId = entry?.Id,
                MoodType = entry == null
                    ? null
                    : ResolveMoodType(entry.PrimaryMood)
            });
        }

        return result;
    }

    public async Task<bool> HasTodayEntryAsync()
    {
        int userId = RequireUser();
        var today = DateTime.Today;

        return await _context.JournalEntries
            .AnyAsync(e =>
                e.UserId == userId &&
                e.CreatedAt.Date == today);
    }

    public async Task<List<DateTime>> GetMissedDaysAsync(int year, int month)
    {
        int userId = RequireUser();

        var today = DateTime.Today;
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        var entries = await _context.JournalEntries
            .Where(e =>
                e.UserId == userId &&
                e.CreatedAt >= start &&
                e.CreatedAt < end)
            .Select(e => e.CreatedAt.Date)
            .ToListAsync();

        var missed = new List<DateTime>();

        for (var d = start; d < end && d < today; d = d.AddDays(1))
        {
            if (!entries.Contains(d))
                missed.Add(d);
        }

        return missed;
    }

    private string ResolveMoodType(string mood)
    {
        return mood switch
        {
            "Happy" or "Excited" or "Relaxed" or "Grateful" or "Confident"
                => "positive",

            "Calm" or "Thoughtful" or "Curious" or "Nostalgic" or "Bored"
                => "neutral",

            _ => "negative"
        };
    }
}
