using System;
using Microsoft.AspNetCore.Mvc;
using SolarWatchApp.JsonProcessor;
using SolarWatchApp.Models;

namespace SolarWatchApp.Controllers;

[ApiController]
[Route("/api/get/[controller]ByLatAndLng")]
public class SunsetSunriseController : ControllerBase
{
    private readonly string baseEndpoint = "https://api.sunrise-sunset.org/json?";
    private readonly HttpClient _httpClient;
    private readonly IJsonProcessor _jsonProcessor;

    public SunsetSunriseController(HttpClient httpClient, IJsonProcessor jsonProcessor)
    {
        _httpClient = httpClient;
        _jsonProcessor = jsonProcessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetByLatLng(double lat, double lng)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"{baseEndpoint}lat={lat}&lng={lng}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var sunrise = _jsonProcessor.GetStringPropertySS(jsonResponse, "sunrise");
                var sunset = _jsonProcessor.GetStringPropertySS(jsonResponse, "sunset");

                var result = new SunsetSunrise(sunset, sunrise);

                return Ok(result);
            }
            else
            {
                throw new Exception($"API request failed with status code {(int)responseMessage.StatusCode}");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}