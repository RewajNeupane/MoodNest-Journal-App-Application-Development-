using System;

public class CalendarDayModel
{
    public DateTime Date { get; set; }
    public int DayNumber => Date.Day;

    public bool IsToday { get; set; }
    public bool HasEntry { get; set; }

    public int? JournalId { get; set; }

    // positive | neutral | negative | null
    public string? MoodType { get; set; }
}