using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SolarWatchApp.Models;

public class SunsetSunrise
{
    [Key]
    public int Id { get; set; }
    public string Sunset { get; set; }
    public string Sunrise { get; set; }

    public int CityId { get; set; }
    [JsonIgnore]
    public City City { get; set; }
    public SunsetSunrise(string sunset, string sunrise)
    {
        Sunset = sunset;
        Sunrise = sunrise;
    }

}