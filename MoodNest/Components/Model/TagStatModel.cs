namespace MoodNest.Components.Model;

/// <summary>
/// Represents usage statistics for a specific tag
/// across a user's journal entries.
/// </summary>
public class TagStatModel
{
    /// <summary>
    /// The tag label (e.g., "#Health", "#Reflection").
    /// </summary>
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// Number of times the tag appears across
    /// all journal entries.
    /// </summary>
    public int Count { get; set; }
}