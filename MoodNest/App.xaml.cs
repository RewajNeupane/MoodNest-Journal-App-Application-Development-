using Microsoft.Maui;
using Microsoft.Maui.Controls;
using MoodNest.Data;

namespace MoodNest;

public partial class App : Application
{
    public App(AppDbContext db)
    {
        InitializeComponent();
        
        db.Database.EnsureCreated();
    }


    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage())
        {
            Title = "MoodNest"
        };
    }
}