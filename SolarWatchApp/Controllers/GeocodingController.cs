using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatchApp.Models;


namespace SolarWatchApp.Controllers;

[ApiController]
[Route("/api/get/[controller]/city-by-lat-lng")]
public class GeocodingController: ControllerBase
{
    private string APIKey = "3c1d6061af22d28e6de4e12694be2734";
    private HttpClient _httpClient;
    private string baseUrl = "http://api.openweathermap.org/geo/1.0/direct?q=";
    private string baseUrlReverse = "http://api.openweathermap.org/geo/1.0/reverse?";
    public GeocodingController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    [HttpGet]
    public IActionResult GetCityByLatAndLng(double lat, double lng)
    {
        //http://api.openweathermap.org/geo/1.0/reverse?lat={lat}&lon={lon}&limit={limit}&appid={API key}
        try
        {
            HttpResponseMessage response =
                _httpClient.GetAsync($"{baseUrlReverse}lat={lat}&lon={lng}&limit=1&appid={APIKey}").Result;
            if (response.IsSuccessStatusCode)
            {
                // Read the JSON response as a string
                string jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Parse the JSON string to a JSON document
                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    // Extract the "name" property
                    var root = doc.RootElement;
                    string cityName = root[0].GetProperty("name").GetString();

                    return Ok(cityName);
                }
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
    public IActionResult GetLatAndLngByCityName(string name)
    {
        //http://api.openweathermap.org/geo/1.0/direct?q=London&limit=5&appid={API key}
        try
        {
            HttpResponseMessage response = _httpClient.GetAsync($"{baseUrl}{name}&limit=1&appid={APIKey}").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStreamAsync().Result;

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    var lat = root[0].GetProperty("lat").GetDouble();
                    var lng = root[0].GetProperty("lon").GetDouble();

                    Location location = new Location(lat, lng);

                    return Ok(location);

                }
                
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