using api_versioning.Requests;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api_versioning.Controllers;

public enum TemperatureUnit
{
    Celsius,
    Fahrenheit,
    Kelvin
}

[ApiController]
[ApiVersion(1.0)]
[ApiVersion(2.0)]
[Route("v{version:apiVersion}/[controller]")]
//[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMediator _mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [MapToApiVersion(1.0)]
    public async Task<ObjectResult> GetForecast()
    {
        Console.WriteLine("called v1 of the api");
        var result = await _mediator.Send(new WeatherForecastRequest<IEnumerable<Contracts.v1.WeatherForecast>>());        
        return new OkObjectResult(result);
    }
    
    [HttpGet]
    [MapToApiVersion(2.0)]
    public async Task<ObjectResult> GetConvertedForecast(TemperatureUnit unit)
    {
        Console.WriteLine("called v2 of the api");
        var request = new WeatherForecastRequest<IEnumerable<Contracts.v2.WeatherForecast>>
        {
            Unit = unit
        };

        var result = await _mediator.Send(request);        
        return new OkObjectResult(result);
    }
}
