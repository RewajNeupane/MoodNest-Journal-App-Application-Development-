using Microsoft.Maui.Storage;

namespace MoodNest.Components.Services;

public class ThemeService
{
    private const string ThemeKey = "app_theme";

    public bool IsDark
    {
        get => Preferences.Get(ThemeKey, false);
        set => Preferences.Set(ThemeKey, value);
    }

    public string CssClass => IsDark ? "dark-theme" : "light-theme";

    public void Toggle()
    {
        IsDark = !IsDark;
    }
}