namespace MoodNest.Components.Model;

public class JournalStatsModel
{
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int MissedDays { get; set; }

    // NEW
    public string? MostFrequentMood { get; set; }

    public int PositivePercentage { get; set; }
    public int NeutralPercentage { get; set; }
    public int NegativePercentage { get; set; }
}