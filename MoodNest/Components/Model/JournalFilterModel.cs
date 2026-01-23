using System;
using System.Collections.Generic;

namespace MoodNest.Components.Model;

public class JournalFilterModel
{
    public string? SearchText { get; set; }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public List<string> Moods { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}