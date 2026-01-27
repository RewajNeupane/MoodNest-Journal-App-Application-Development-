using System.ComponentModel.DataAnnotations;

namespace MoodNest.Models;

/// <summary>
/// Represents user input required for account registration.
/// Includes validation rules to ensure data correctness
/// and basic security constraints.
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// Username chosen by the user.
    /// Must contain at least 3 characters.
    /// </summary>
    [Required]
    [MinLength(3)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the user.
    /// Must follow a valid email format.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Personal Identification Number (PIN) used for authentication.
    /// Must consist of exactly 4 numeric digits.
    /// </summary>
    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be exactly 4 digits")]
    public string Pin { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation field for the PIN.
    /// Must match the value entered in the Pin field.
    /// </summary>
    [Required]
    [Compare(nameof(Pin), ErrorMessage = "PINs do not match")]
    public string ConfirmPin { get; set; } = string.Empty;
}