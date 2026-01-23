using System;
using System.ComponentModel.DataAnnotations;

namespace MoodNest.Components.Model;

public class JournalEntryViewModel
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Journal content cannot be empty")]
    public string ContentHtml { get; set; } = string.Empty;

    [Required]
    public string PrimaryMood { get; set; } = string.Empty;

    // Comma-separated values (e.g. "😴 Tired,😬 Anxious")
    public string? SecondaryMoods { get; set; }

    [Required]
    public string Category { get; set; } = "Personal";

    // Comma-separated tags (e.g. "#reflection,#health")
    public string? Tags { get; set; }

    // ✅ REQUIRED FOR EDIT MODE + TIMESTAMPS UI
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}