using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoodNest.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PinHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    // 🔗 Journals
    public List<JournalEntry> JournalEntries { get; set; } = new();
}