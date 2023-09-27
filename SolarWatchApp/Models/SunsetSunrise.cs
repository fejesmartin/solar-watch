namespace SolarWatchApp.Models;

public class SunsetSunrise
{
    public string Sunset { get; set; }
    public string Sunrise { get; set; }

    public SunsetSunrise(string sunset, string sunrise)
    {
        Sunset = sunset;
        Sunrise = sunrise;
    }

}