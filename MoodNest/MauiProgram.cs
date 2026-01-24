using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.Hosting;
using MoodNest.Components.Services;
using MoodNest.Data;


namespace MoodNest;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        SQLitePCL.Batteries.Init();



        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // Services
        builder.Services.AddScoped<PinAuthService>();
        builder.Services.AddScoped<IJournalService, JournalService>();
        builder.Services.AddScoped<IJournalFilterService, JournalFilterService>();
        builder.Services.AddScoped<ICalendarService, CalendarService>();
        builder.Services.AddSingleton<ThemeService>();
        builder.Services.AddScoped<JournalPdfExportService>(); // ✅ IMPORTANT

        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "moodnest.db"
        );

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}")
        );

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}