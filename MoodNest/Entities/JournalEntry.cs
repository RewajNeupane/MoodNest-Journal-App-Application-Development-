using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodNest.Entities;

public class JournalEntry
{
    [Key]
    public int Id { get; set; }

    // 🔗 OWNER
    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = "Untitled Entry";

    [Required]
    public string ContentHtml { get; set; } = string.Empty;

    public string PrimaryMood { get; set; } = string.Empty;
    public string? SecondaryMoods { get; set; }

    public string Category { get; set; } = "Personal";
    public string? Tags { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}