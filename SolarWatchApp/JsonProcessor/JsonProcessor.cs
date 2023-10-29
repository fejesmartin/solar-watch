using System.Text.Json;

namespace SolarWatchApp.JsonProcessor;

public class JsonProcessor : IJsonProcessor
{
    public string GetStringPropertySS(string json, string propertyName)
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            var results = root.GetProperty("results");
            return results.GetProperty(propertyName).GetString();
        }
    }

    public double GetDoublePropertySS(string json, string propertyName)
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            var results = root.GetProperty("results");
            return results.GetProperty(propertyName).GetDouble();
        }
    }

    public string GetStringPropertyGeo(string json, string propertyName)
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            return root[0].GetProperty(propertyName).GetString();
        }
    }
    
    public double GetDoublePropertyGeo(string json, string propertyName)
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            return root[0].GetProperty(propertyName).GetDouble();
        }
    }
    
    public string GetStringProperty(string json, string propertyName)
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            return root.GetProperty(propertyName).GetString();
        }
    }
    
    public double GetDoubleProperty(string json, string propertyName)
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            return root.GetProperty(propertyName).GetDouble();
        }
    }
}