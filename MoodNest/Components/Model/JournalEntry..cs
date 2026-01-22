using System;
using System.ComponentModel.DataAnnotations;

namespace MoodNest.Components.Model;

public class JournalEntry
{
    [Key]
    public int Id { get; set; }

    // ✅ NEW: Extracted from first <h1> in Quill
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = "Untitled Entry";

    [Required]
    public string ContentHtml { get; set; } = string.Empty;

    public string PrimaryMood { get; set; } = string.Empty;

    // Comma-separated values (e.g. "😴 Tired,😬 Anxious")
    public string? SecondaryMoods { get; set; }

    public string Category { get; set; } = "Personal";

    // Comma-separated tags (e.g. "#reflection,#health")
    public string? Tags { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}