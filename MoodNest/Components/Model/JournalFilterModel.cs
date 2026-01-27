using System;
using System.Collections.Generic;

namespace MoodNest.Components.Model;

/// <summary>
/// Represents filter criteria used to query and refine
/// journal entries based on user-selected conditions.
/// </summary>
public class JournalFilterModel
{
    /// <summary>
    /// Free-text search input used to match journal titles
    /// or content.
    /// </summary>
    public string? SearchText { get; set; }

    /// <summary>
    /// Optional start date for filtering journal entries.
    /// Entries created before this date are excluded.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Optional end date for filtering journal entries.
    /// Entries created after this date are excluded.
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Collection of moods used to filter journal entries.
    /// Only entries matching one of these moods are included.
    /// </summary>
    public List<string> Moods { get; set; } = new();

    /// <summary>
    /// Collection of tags used to filter journal entries.
    /// Only entries containing one or more of these tags
    /// are included.
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Optional category filter.
    /// When specified, only entries belonging to this
    /// category are included.
    /// </summary>
    public string? Category { get; set; }
}