namespace SolarWatchApp.Models;

public class SolarWatch
{
    public string Name { get; set; }
    public string Sunset { get; set; }
    public string Sunrise { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public SolarWatch(string name, string sunset, string sunrise, double latitude, double longitude)
    {
        Name = name;
        Sunset = sunset;
        Sunrise = sunrise;
        Latitude = latitude;
        Longitude = longitude;
    }
}