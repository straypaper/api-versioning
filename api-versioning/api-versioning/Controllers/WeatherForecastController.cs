using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace api_versioning.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("v{version:apiVersion}/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    public IEnumerable<WeatherForecast> GetForecast()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet]
    [MapToApiVersion("2.0")]
    public IEnumerable<WeatherForecastv2> GetConvertedForecast(TemperatureUnit unit)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecastv2
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Temperature = ConvertToUnit(unit, Random.Shared.Next(-20, 55)),
                
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    private static double ConvertToUnit(TemperatureUnit unit, int next)
    {
        return unit switch
        {
            TemperatureUnit.Celsius => next,
            TemperatureUnit.Fahrenheit => 32 + (int)(next / 0.5556),
            TemperatureUnit.Kelvin => Convert.ToDouble(next) + 273.15,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }
}

public enum TemperatureUnit
{
    Celsius,
    Fahrenheit,
    Kelvin
}