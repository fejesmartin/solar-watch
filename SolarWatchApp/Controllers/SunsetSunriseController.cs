using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatchApp.Models;

namespace SolarWatchApp.Controllers;

[ApiController]
[Route("/api/get/[controller]ByLatAndLng")]
public class SunsetSunriseController: ControllerBase
{
    private string baseEndpoint = "https://api.sunrise-sunset.org/json?";
    private HttpClient _httpClient;

    public SunsetSunriseController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByLatLng(double lat, double lng)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"{baseEndpoint}lat={lat}&lng={lng}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    var sunrise = root.GetProperty("results").GetProperty("sunrise").GetString();
                    var sunset = root.GetProperty("results").GetProperty("sunset").GetString();
                    
                    var result = new SunsetSunrise(sunset,sunrise);

                    return Ok(result);

                }
            }
            else
            {
                return StatusCode((int)responseMessage.StatusCode, "Error message");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
       
    }
}