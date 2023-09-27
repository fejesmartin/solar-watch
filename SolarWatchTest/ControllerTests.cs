namespace SolarWatchTest;

public class ControllerTests
{
    private HttpClient _httpClient;
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }
    
    [Test]
    public async Task GeocodingControllerReturnsWith200StatusCode()
    {
        HttpResponseMessage responseMessage = await _httpClient
            .GetAsync("http://localhost:5174/api/get/Geocoding/city-by-lat-lng/lat-lng-by-cityname?name=London");
        
        Assert.That(responseMessage.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}