using System.Collections.Generic;

namespace MoodNest.Components.Model;

public class AnalyticsResultModel
{
    // Mood
    public string? MostFrequentMood { get; set; }

    public int PositivePercentage { get; set; }
    public int NeutralPercentage { get; set; }
    public int NegativePercentage { get; set; }

    // Streaks
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int MissedDays { get; set; }

    // Tags
    public List<TagStatModel> MostUsedTags { get; set; } = new();
    public List<CategoryStatModel> TagBreakdown { get; set; } = new();

    // Writing
    public List<WordTrendModel> WordTrends { get; set; } = new();
}