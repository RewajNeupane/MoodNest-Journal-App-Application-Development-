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
        // ✅ REQUIRED for EF Core + SQLite on MacCatalyst
        SQLitePCL.Batteries.Init();

        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // MAUI Blazor
        builder.Services.AddMauiBlazorWebView();

        // PIN auth
        builder.Services.AddScoped<PinAuthService>();
        
        //  JOURNAL SERVICE 
        builder.Services.AddScoped<IJournalService, JournalService>();
        
        // JOURNAL FILTERING
        builder.Services.AddScoped<IJournalFilterService, JournalFilterService>();
        
        // THEME SERVICE
        builder.Services.AddSingleton<ThemeService>();


        // EF Core SQLite
        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "moodnest.db"
        );
        // 🔎 DEBUG: print DB path (temporary)
        Console.WriteLine($"DB PATH => {dbPath}");
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