using System;

/// <summary>
/// Represents a single day within the calendar view,
/// including journal availability and mood metadata.
/// </summary>
public class CalendarDayModel
{
    /// <summary>
    /// The calendar date represented by this model.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Numeric day value extracted from the Date (1–31).
    /// Useful for calendar display.
    /// </summary>
    public int DayNumber => Date.Day;

    /// <summary>
    /// Indicates whether this date corresponds to the current system date.
    /// </summary>
    public bool IsToday { get; set; }

    /// <summary>
    /// Indicates whether a journal entry exists for this date.
    /// </summary>
    public bool HasEntry { get; set; }

    /// <summary>
    /// Identifier of the associated journal entry, if one exists.
    /// Null when no entry is present for the date.
    /// </summary>
    public int? JournalId { get; set; }

    /// <summary>
    /// Categorized mood type for the day.
    /// Expected values: "positive", "neutral", "negative", or null.
    /// Used for calendar visual indicators.
    /// </summary>
    public string? MoodType { get; set; }
}