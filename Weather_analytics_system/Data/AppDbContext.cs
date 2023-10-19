using Microsoft.EntityFrameworkCore;
using Weather_analytics_system.Models;

namespace Weather_analytics_system.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
        Database.Migrate();
    }
    public DbSet<Measurement> Measurements { get; set; } = null!;
}