using System.Collections.Generic;

namespace MoodNest.Components.Model;

/// <summary>
/// Represents aggregated analytics data derived from a user's journal entries.
/// This model is used to display insights such as mood distribution, writing
/// streaks, tag usage, and word trends.
/// </summary>
public class AnalyticsResultModel
{
    // ===============================
    // MOOD ANALYTICS
    // ===============================

    /// <summary>
    /// The most frequently occurring primary mood across all journal entries.
    /// </summary>
    public string? MostFrequentMood { get; set; }

    /// <summary>
    /// Percentage of entries classified as having a positive mood.
    /// </summary>
    public int PositivePercentage { get; set; }

    /// <summary>
    /// Percentage of entries classified as having a neutral mood.
    /// </summary>
    public int NeutralPercentage { get; set; }

    /// <summary>
    /// Percentage of entries classified as having a negative mood.
    /// </summary>
    public int NegativePercentage { get; set; }


    // ===============================
    // STREAK & CONSISTENCY METRICS
    // ===============================

    /// <summary>
    /// Number of consecutive days the user has written journal entries
    /// up to the current date.
    /// </summary>
    public int CurrentStreak { get; set; }

    /// <summary>
    /// Longest continuous streak of daily journal entries achieved by the user.
    /// </summary>
    public int LongestStreak { get; set; }

    /// <summary>
    /// Total number of days where no journal entry was written
    /// within the tracked period.
    /// </summary>
    public int MissedDays { get; set; }


    // ===============================
    // TAG & CATEGORY ANALYTICS
    // ===============================

    /// <summary>
    /// List of the most frequently used tags across all journal entries,
    /// ordered by usage count.
    /// </summary>
    public List<TagStatModel> MostUsedTags { get; set; } = new();

    /// <summary>
    /// Breakdown of journal entries by category, including usage statistics.
    /// </summary>
    public List<CategoryStatModel> TagBreakdown { get; set; } = new();


    // ===============================
    // WRITING ANALYTICS
    // ===============================

    /// <summary>
    /// Historical word count trends showing writing volume over time.
    /// </summary>
    public List<WordTrendModel> WordTrends { get; set; } = new();
}
