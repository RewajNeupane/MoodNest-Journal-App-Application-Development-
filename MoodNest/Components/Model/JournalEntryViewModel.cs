using System;
using System.ComponentModel.DataAnnotations;

namespace MoodNest.Components.Model;

/// <summary>
/// Represents the data required to create, edit, and display
/// a full journal entry within the application.
/// Used primarily for journal editor and detail views.
/// </summary>
public class JournalEntryViewModel
{
    /// <summary>
    /// Title of the journal entry.
    /// Maximum length is limited to 200 characters.
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// HTML-formatted body content of the journal entry.
    /// Cannot be empty.
    /// </summary>
    [Required(ErrorMessage = "Journal content cannot be empty")]
    public string ContentHtml { get; set; } = string.Empty;

    /// <summary>
    /// Primary mood selected for the journal entry.
    /// Used as the main emotional indicator.
    /// </summary>
    [Required]
    public string PrimaryMood { get; set; } = string.Empty;

    /// <summary>
    /// Optional list of secondary moods associated with the entry.
    /// Stored as a comma-separated string
    /// (e.g., "😴 Tired, 😬 Anxious").
    /// </summary>
    public string? SecondaryMoods { get; set; }

    /// <summary>
    /// Category assigned to the journal entry.
    /// Defaults to "Personal".
    /// </summary>
    [Required]
    public string Category { get; set; } = "Personal";

    /// <summary>
    /// Optional tags assigned to the journal entry.
    /// Stored as a comma-separated string
    /// (e.g., "#reflection,#health").
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Timestamp indicating when the journal entry was created.
    /// Required for edit mode and timeline display.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp indicating when the journal entry was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Indicates whether the journal entry is publicly visible
    /// in the shared journals feed.
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    /// Display name of the author who created the journal entry.
    /// Used primarily for shared/public views.
    /// </summary>
    public string? Author { get; set; }
}
