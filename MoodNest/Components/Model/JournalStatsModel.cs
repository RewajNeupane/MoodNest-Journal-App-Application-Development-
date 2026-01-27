namespace MoodNest.Components.Model;

/// <summary>
/// Represents statistical insights derived from a user's
/// journaling activity, including streaks and mood distribution.
/// </summary>
public class JournalStatsModel
{
    // ===============================
    // STREAK METRICS
    // ===============================

    /// <summary>
    /// Number of consecutive days the user has written
    /// a journal entry up to the current date.
    /// </summary>
    public int CurrentStreak { get; set; }

    /// <summary>
    /// Longest continuous streak of daily journal entries
    /// achieved by the user.
    /// </summary>
    public int LongestStreak { get; set; }

    /// <summary>
    /// Total number of days where no journal entry was written
    /// during the tracked period.
    /// </summary>
    public int MissedDays { get; set; }


    // ===============================
    // MOOD ANALYTICS
    // ===============================

    /// <summary>
    /// The most frequently occurring primary mood across
    /// all journal entries.
    /// </summary>
    public string? MostFrequentMood { get; set; }

    /// <summary>
    /// Percentage of journal entries classified as positive.
    /// </summary>
    public int PositivePercentage { get; set; }

    /// <summary>
    /// Percentage of journal entries classified as neutral.
    /// </summary>
    public int NeutralPercentage { get; set; }

    /// <summary>
    /// Percentage of journal entries classified as negative.
    /// </summary>
    public int NegativePercentage { get; set; }
}