namespace SolarWatchApp.Models;

public class SolarWatch
{
    public string Name { get; set; }
    public string Sunset { get; set; }
    public string SunRise { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public SolarWatch(string name, string sunset, string sunRise, double latitude, double longitude)
    {
        Name = name;
        Sunset = sunset;
        SunRise = sunRise;
        Latitude = latitude;
        Longitude = longitude;
    }
}