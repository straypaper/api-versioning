using api_versioning.Controllers;
using api_versioning.Requests;
using MediatR;

namespace api_versioning.Handlers;

public class WeatherForecastHandler : 
    IRequestHandler<WeatherForecastRequest<IEnumerable<Contracts.v1.WeatherForecast>>, IEnumerable<Contracts.v1.WeatherForecast>>,
    IRequestHandler<WeatherForecastRequest<IEnumerable<Contracts.v2.WeatherForecast>>, IEnumerable<Contracts.v2.WeatherForecast>>
{
    private static readonly string[] Summaries = 
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    public Task<IEnumerable<Contracts.v1.WeatherForecast>> Handle(WeatherForecastRequest<IEnumerable<Contracts.v1.WeatherForecast>> request, CancellationToken cancellationToken)
    {
        var result = Enumerable.Range(1, 5).Select(index => new Contracts.v1.WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        });

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Contracts.v2.WeatherForecast>> Handle(WeatherForecastRequest<IEnumerable<Contracts.v2.WeatherForecast>> request, CancellationToken cancellationToken)
    {
        var result = Enumerable.Range(1, 5).Select(index => new Contracts.v2.WeatherForecast
        {
            Unit = request.Unit.ToString(),
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Temperature = ConvertToUnit(request.Unit, Random.Shared.Next(-20, 55)),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        });

        return Task.FromResult(result);
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