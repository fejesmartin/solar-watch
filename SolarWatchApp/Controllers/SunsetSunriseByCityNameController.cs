using System;
using Microsoft.AspNetCore.Mvc;
using SolarWatchApp.JsonProcessor;
using SolarWatchApp.Models;

namespace SolarWatchApp.Controllers;

[ApiController]
[Route("/api/get/[controller]")]
public class SunsetSunriseByCityNameController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IJsonProcessor _jsonProcessor;

    public SunsetSunriseByCityNameController(HttpClient httpClient, IJsonProcessor jsonProcessor)
    {
        _httpClient = httpClient;
        _jsonProcessor = jsonProcessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetSRiseAndSRetByCityName(string name)
    {
        try
        {
            var location = await GetLocationByCityName(name);
            var sunsetSunrise = await GetSunsetSunriseByLatAndLng(location.Lat, location.Lng);

            var solarWatch = new SolarWatch(name, sunsetSunrise.Sunset, sunsetSunrise.Sunrise, location.Lat, location.Lng);
            return Ok(solarWatch);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private async Task<Location> GetLocationByCityName(string name)
    {
        var cityResponse = await _httpClient
            .GetAsync($"http://localhost:5174/api/get/Geocoding/city-by-lat-lng/lat-lng-by-cityname?name={name}");

        if (cityResponse.IsSuccessStatusCode)
        {
            var jsonResponseCity = await cityResponse.Content.ReadAsStringAsync();
            var lat = _jsonProcessor.GetDoubleProperty(jsonResponseCity, "lat");
            var lng = _jsonProcessor.GetDoubleProperty(jsonResponseCity, "lng");
            return new Location(lat, lng);
        }
        else
        {
            throw new Exception($"City API request failed with status code {(int)cityResponse.StatusCode}");
        }
    }

    private async Task<SunsetSunrise> GetSunsetSunriseByLatAndLng(double lat, double lng)
    {
        var sunResponse = await _httpClient.GetAsync($"http://localhost:5174/api/get/SunsetSunriseByLatAndLng?lat={lat}&lng={lng}");

        if (sunResponse.IsSuccessStatusCode)
        {
            var jsonResponseSun = await sunResponse.Content.ReadAsStringAsync();
            var sunset = _jsonProcessor.GetStringProperty(jsonResponseSun, "sunset");
            var sunrise = _jsonProcessor.GetStringProperty(jsonResponseSun, "sunrise");
            return new SunsetSunrise(sunset, sunrise);
        }
        else
        {
            throw new Exception($"SunsetSunrise API request failed with status code {(int)sunResponse.StatusCode}");
        }
    }
}
