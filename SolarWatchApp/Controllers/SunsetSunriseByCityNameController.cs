using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatchApp.Models;

namespace SolarWatchApp.Controllers;

[ApiController]
[Route("/api/get/[controller]")]
public class SunsetSunriseByCityNameController: ControllerBase
{
    private HttpClient _httpClient;

    public SunsetSunriseByCityNameController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetSRiseAndSRetByCityName(string name)
    {
        try
        {
            HttpResponseMessage cityResponse = await _httpClient
                .GetAsync($"http://localhost:8080/api/get/Geocoding/city-by-lat-lng/lat-lng-by-cityname?name={name}");
            var location = new Location(0, 0);
            var sunsetSunrise = new SunsetSunrise("","");
            
            if (cityResponse.IsSuccessStatusCode)
            {
                var jsonResponseCity = await cityResponse.Content.ReadAsStreamAsync();

                using (JsonDocument doc = await JsonDocument.ParseAsync(jsonResponseCity))
                {
                    var root = doc.RootElement;
                    var lat = root.GetProperty("lat").GetDouble();
                    var lng = root.GetProperty("lng").GetDouble();
                    location.Lat = lat;
                    location.Lng = lng;
                }

                HttpResponseMessage sunResponse = _httpClient.GetAsync($"http://localhost:5174/api/get/SunsetSunriseByLatAndLng?lat={location.Lat}&lng={location.Lng}").Result;

                if (sunResponse.IsSuccessStatusCode)
                {
                    var jsonResponseSun = sunResponse.Content.ReadAsStreamAsync().Result;

                    using (JsonDocument document = JsonDocument.Parse(jsonResponseSun))
                    {
                        var root = document.RootElement;
                        var sunset = root.GetProperty("sunset").GetString();
                        var sunrise = root.GetProperty("sunrise").GetString();
                        sunsetSunrise.Sunset = sunset;
                        sunsetSunrise.Sunrise = sunrise;
                    }
                }

                SolarWatch solarWatch = new SolarWatch(name,sunsetSunrise.Sunset, sunsetSunrise.Sunrise,location.Lat,location.Lng);
                return Ok(solarWatch);
            }
            else
            {
                return StatusCode((int)cityResponse.StatusCode, "error");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}