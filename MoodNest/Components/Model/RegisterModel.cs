using System.ComponentModel.DataAnnotations;

namespace MoodNest.Models;

public class RegisterModel
{
    [Required, MinLength(3)]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be exactly 4 digits")]
    public string Pin { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Pin), ErrorMessage = "PINs do not match")]
    public string ConfirmPin { get; set; } = string.Empty;
}