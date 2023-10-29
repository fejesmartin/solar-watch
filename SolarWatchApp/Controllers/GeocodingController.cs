using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatchApp.JsonProcessor;
using SolarWatchApp.Models;


namespace SolarWatchApp.Controllers;

[ApiController]
[Route("/api/get/[controller]/city-by-lat-lng")]
public class GeocodingController : ControllerBase
{
    private readonly string APIKey = "3c1d6061af22d28e6de4e12694be2734";
    private readonly HttpClient _httpClient;
    private readonly string baseUrl = "http://api.openweathermap.org/geo/1.0/direct?q=";
    private readonly string baseUrlReverse = "http://api.openweathermap.org/geo/1.0/reverse?";
    private readonly IJsonProcessor _jsonProcessor;

    public GeocodingController(HttpClient httpClient, IJsonProcessor jsonProcessor)
    {
        _httpClient = httpClient;
        _jsonProcessor = jsonProcessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetCityByLatAndLng(double lat, double lng)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{baseUrlReverse}lat={lat}&lon={lng}&limit=1&appid={APIKey}");
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                
                    // Extract the "name" property
                    var cityName = _jsonProcessor.GetStringPropertyGeo(jsonResponse, "name");

                    return Ok(cityName);
                
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error message");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("lat-lng-by-cityname")]
    public async Task<IActionResult> GetLatAndLngByCityName(string name)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{baseUrl}{name}&limit=1&appid={APIKey}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var lat = _jsonProcessor.GetDoublePropertyGeo(jsonResponse, "lat");
                var lng = _jsonProcessor.GetDoublePropertyGeo(jsonResponse, "lon");
                Location location = new Location(lat, lng);
                return Ok(location);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}