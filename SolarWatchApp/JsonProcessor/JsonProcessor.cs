using System.Text.Json;

namespace SolarWatchApp.JsonProcessor;

public class JsonProcessor: IJsonProcessor
{
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