namespace MoodNest.Components.Model;

/// <summary>
/// Represents a lightweight, read-only projection of a journal entry
/// used for list views, timelines, and shared journal displays.
/// </summary>
public class JournalEntryDisplayModel
{
    /// <summary>
    /// Unique identifier of the journal entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the journal entry.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Primary mood associated with the entry.
    /// Typically displayed with an emoji (e.g., 😊, 😔, 😠).
    /// </summary>
    public string PrimaryMood { get; set; } = string.Empty;

    /// <summary>
    /// Optional list of secondary moods associated with the entry.
    /// Stored as a comma-separated string (e.g., "😴 Tired, 😬 Anxious").
    /// </summary>
    public string? SecondaryMoods { get; set; }

    /// <summary>
    /// Category assigned to the journal entry
    /// (e.g., "Personal", "Work", "Health").
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Optional tags associated with the entry.
    /// Stored as a comma-separated string (e.g., "#Health,#Reflection").
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Day component of the entry's creation date.
    /// Used for calendar and timeline display.
    /// </summary>
    public int Day { get; set; }

    /// <summary>
    /// Full month name of the entry's creation date
    /// (e.g., "January", "February").
    /// </summary>
    public string Month { get; set; } = string.Empty;

    /// <summary>
    /// Year component of the entry's creation date.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Display name of the author who created the journal entry.
    /// Used primarily for shared/public journal views.
    /// </summary>
    public string Author { get; set; } = string.Empty;
}
