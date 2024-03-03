using api_versioning.Controllers;

namespace api_versioning.Contracts.v2;

public class WeatherForecast
{
    public string Unit { get; set; }
    public DateOnly Date { get; set; }

    public double Temperature { get; set; }
}
