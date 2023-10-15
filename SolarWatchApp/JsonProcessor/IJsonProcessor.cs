using System.Text.Json;

namespace SolarWatchApp.JsonProcessor;

public interface IJsonProcessor
{
    public string GetStringPropertySS(string json, string propertyName);
    public double GetDoublePropertySS(string json, string propertyName);
    public string GetStringPropertyGeo(string json, string propertyName);
    public double GetDoublePropertyGeo(string json, string propertyName);
    public string GetStringProperty(string json, string propertyName);
    public double GetDoubleProperty(string json, string propertyName);
}