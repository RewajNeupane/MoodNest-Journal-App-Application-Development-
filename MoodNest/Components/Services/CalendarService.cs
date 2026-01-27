using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoodNest.Data;
using MoodNest.Components.Services;

/// <summary>
/// Service responsible for providing calendar-related data
/// such as monthly entries, today status, and missed days.
/// </summary>
public class CalendarService : ICalendarService
{
    private readonly AppDbContext _context;
    private readonly PinAuthService _auth;

    /// <summary>
    /// Initializes the calendar service with database context
    /// and authentication service.
    /// </summary>
    public CalendarService(AppDbContext context, PinAuthService auth)
    {
        _context = context;
        _auth = auth;
    }

    /// <summary>
    /// Ensures a user is authenticated and returns the user ID.
    /// Throws an exception if the user is not logged in.
    /// </summary>
    private int RequireUser()
    {
        if (_auth.CurrentUserId == null)
            throw new InvalidOperationException("User not authenticated");

        return _auth.CurrentUserId.Value;
    }

    /// <summary>
    /// Returns calendar data for a specific month, including
    /// entry presence, mood type, and journal ID.
    /// </summary>
    public async Task<List<CalendarDayModel>> GetMonthAsync(int year, int month)
    {
        int userId = RequireUser();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        // Fetch all journal entries for the given month
        var entries = await _context.JournalEntries
            .Where(e =>
                e.UserId == userId &&
                e.CreatedAt >= start &&
                e.CreatedAt < end)
            .ToListAsync();

        var daysInMonth = DateTime.DaysInMonth(year, month);
        var today = DateTime.Today;

        var result = new List<CalendarDayModel>();

        // Build calendar day models
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

    /// <summary>
    /// Checks whether the authenticated user has already
    /// written a journal entry today.
    /// </summary>
    public async Task<bool> HasTodayEntryAsync()
    {
        int userId = RequireUser();
        var today = DateTime.Today;

        return await _context.JournalEntries
            .AnyAsync(e =>
                e.UserId == userId &&
                e.CreatedAt.Date == today);
    }

    /// <summary>
    /// Returns a list of past dates in the given month
    /// where no journal entry was written.
    /// </summary>
    public async Task<List<DateTime>> GetMissedDaysAsync(int year, int month)
    {
        int userId = RequireUser();

        var today = DateTime.Today;
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        // Fetch all entry dates for the month
        var entries = await _context.JournalEntries
            .Where(e =>
                e.UserId == userId &&
                e.CreatedAt >= start &&
                e.CreatedAt < end)
            .Select(e => e.CreatedAt.Date)
            .ToListAsync();

        var missed = new List<DateTime>();

        // Identify days before today without entries
        for (var d = start; d < end && d < today; d = d.AddDays(1))
        {
            if (!entries.Contains(d))
                missed.Add(d);
        }

        return missed;
    }

    /// <summary>
    /// Maps a primary mood string to a simplified
    /// mood category for UI visualization.
    /// </summary>
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
