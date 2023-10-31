using System.Net;
using NUnit.Framework;
using SolarWatchApp.Models;
using Newtonsoft.Json;
using SolarWatchIntegrationTest;


namespace SolarWatchApp.IntegrationTests
{
    [TestFixture]
    public class CityControllerIntegrationTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory _factory;
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [Test]
        public async Task GetCityByName_ExistingCity_ReturnsCity()
        {
            
            string cityName = "New York";
            
            var response = await _client.GetAsync($"/api/get/City?name={cityName}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var city = JsonConvert.DeserializeObject<City>(content);
            
            Assert.IsNotNull(city);
            Assert.That(city.Name, Is.EqualTo(cityName));
       
        }

        [Test]
        public async Task GetCityByName_IncorrectCity_ReturnsBadRequest()
        {
            
            string cityName = "SomeNonExistingCity"; // Replace with a non-existing city name
            
            var response = await _client.GetAsync($"/api/get/City?name={cityName}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}