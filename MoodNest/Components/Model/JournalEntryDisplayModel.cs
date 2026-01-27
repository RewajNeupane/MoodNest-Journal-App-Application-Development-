namespace MoodNest.Components.Model;

public class JournalEntryDisplayModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    // 😊 😔 😠
    public string PrimaryMood { get; set; } = string.Empty;

    // "😴 Tired,😬 Anxious"
    public string? SecondaryMoods { get; set; }

    // "Personal", "Work", etc.
    public string Category { get; set; } = string.Empty;

    // "#Health,#Reflection"
    public string? Tags { get; set; }

    // Clean date parts
    public int Day { get; set; }

    // FULL month name: January, February, etc.
    public string Month { get; set; } = string.Empty;

    public int Year { get; set; }
    
    public string Author { get; set; } = string.Empty;

}