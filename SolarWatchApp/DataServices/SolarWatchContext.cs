using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SolarWatchApp.Models;

namespace SolarWatchApp.DataServices;

public class SolarWatchContext: DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunsetSunrise> SunsetSunrises { get; set; }

    public SolarWatchContext(DbContextOptions<SolarWatchContext> options): base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTIONSTRING");
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<City>()
            .HasOne(city => city.SunsetSunrise)
            .WithOne(sunrise => sunrise.City)
            .HasForeignKey<SunsetSunrise>(sunrise => sunrise.CityId);
    }
}