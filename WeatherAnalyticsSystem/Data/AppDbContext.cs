using Microsoft.EntityFrameworkCore;
using WeatherAnalyticsSystem.Models;

namespace WeatherAnalyticsSystem.Data;

public class AppDbContext : DbContext
{
    public DbSet<Weather> Weathers { get; set; } = null!;
    public DbSet<Coord> Coords { get; set; } = null!;
    public DbSet<Sys> Syss { get; set; } = null!;
    public DbSet<Main> Mains { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
        //Database.Migrate();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Weather>()
            .HasOne(w => w.Sys)
            .WithOne()
            .HasForeignKey<Weather>(w => w.Id);

        modelBuilder.Entity<Weather>()
            .HasOne(w => w.Coord)
            .WithOne()
            .HasForeignKey<Weather>(w => w.Id);

        modelBuilder.Entity<Weather>()
            .HasOne(w => w.Main)
            .WithOne()
            .HasForeignKey<Weather>(w => w.Id);
    }
}