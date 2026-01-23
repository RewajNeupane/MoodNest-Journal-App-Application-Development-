using Microsoft.EntityFrameworkCore;
using MoodNest.Entities;

namespace MoodNest.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
}