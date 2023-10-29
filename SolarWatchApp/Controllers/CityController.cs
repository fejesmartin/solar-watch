using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatchApp.DataServices.Repositories;
using SolarWatchApp.JsonProcessor;
using SolarWatchApp.Models;

namespace SolarWatchApp.Controllers;

[ApiController]
[Route("/api/get/[controller]")]
public class CityController : ControllerBase
{
    private readonly string APIKey = "3c1d6061af22d28e6de4e12694be2734";
    private readonly ICityRepository _cityRepository;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly HttpClient _httpClient;

    public CityController(ICityRepository cityRepository, IJsonProcessor jsonProcessor, HttpClient httpClient)
    {
        _cityRepository = cityRepository;
        _jsonProcessor = jsonProcessor;
        _httpClient = httpClient;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetCityByName(string name)
    {
        try
        {
            // Check if city data exists in the repository or the database
            var city = await _cityRepository.GetCityByNameAsync(name);

            if (city != null)
            {
                return Ok(city);
            }
            else
            {
                // Make API call to fetch city data
                var cityData = await FetchCityDataFromAPI(name);

                if (cityData != null)
                {
                    // Save city data to the repository and the database
                    await _cityRepository.CreateCityAsync(cityData);

                    return Ok(cityData);
                }
                else
                {
                    return NotFound("City not found");
                }
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private async Task<City> FetchCityDataFromAPI(string cityName)
    {

        // Make an API call to fetch city data
        var response = await _httpClient.GetAsync($"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={APIKey}");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Use the JSON processor to extract city data
            var lat = _jsonProcessor.GetDoublePropertyGeo(jsonResponse, "lat");
            var lon = _jsonProcessor.GetDoublePropertyGeo(jsonResponse, "lon");
            var country = _jsonProcessor.GetStringPropertyGeo(jsonResponse, "country");
            var state = _jsonProcessor.GetStringPropertyGeo(jsonResponse, "state");
            // Create a new City object
            var cityData = new City
            {
                Name = cityName,
                Latitude = lat,
                Longitude = lon,
                State = state,
                Country = country
                
            };

            return cityData;
        }

        return null; // Data not found or API request failed
    }
}