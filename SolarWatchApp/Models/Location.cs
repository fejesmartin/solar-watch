namespace SolarWatchApp.Models;

public class Location
{
    public double Lat { get; set; }
    public double Lng { get; set; }

    public Location(double lat, double lng)
    {
        Lat = lat;
        Lng = lng;
    }
}