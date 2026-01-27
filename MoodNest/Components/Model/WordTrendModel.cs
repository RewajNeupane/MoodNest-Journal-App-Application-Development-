using System;

namespace MoodNest.Components.Model;

/// <summary>
/// Represents a single data point in a word count trend,
/// used to analyze writing volume over time.
/// </summary>
public class WordTrendModel
{
    /// <summary>
    /// Date associated with the word count measurement.
    /// Typically corresponds to the journal entry creation date.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Number of words written on the specified date.
    /// </summary>
    public int WordCount { get; set; }
}