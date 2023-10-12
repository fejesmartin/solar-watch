using System.ComponentModel.DataAnnotations;

namespace SolarWatchApp.Models;

public class City
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public SunsetSunrise SunsetSunrise { get; set; }
}