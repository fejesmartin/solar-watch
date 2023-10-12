namespace SolarWatchApp.JsonProcessor;

public interface IJsonProcessor
{
    public string GetStringProperty(string json, string propertyName);
    public double GetDoubleProperty(string json, string propertyName);
}