using System.Text.Json.Serialization;
using api_versioning.Requests;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api_versioning.Controllers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TemperatureUnit
{
    Celsius,
    Fahrenheit,
    Kelvin
}

[ApiController]
[ApiVersion(1.0, Deprecated = false)]
[ApiVersion(2.0, Deprecated = false)]
[Route("[controller]")]
//[Route("v{version:apiVersion}/[controller]")]
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
    [ProducesResponseType<Contracts.v1.WeatherForecast>(200, "application/json")]
    public async Task<ObjectResult> GetForecast()
    {
        var result = await _mediator.Send(new WeatherForecastRequest<IEnumerable<Contracts.v1.WeatherForecast>>());        
        return new OkObjectResult(result);
    }
    
    [HttpGet]
    [MapToApiVersion(2.0)]
    [ProducesResponseType<Contracts.v2.WeatherForecast>(200, "application/json")]
    public async Task<ObjectResult> GetConvertedForecast(TemperatureUnit unit)
    {
        var request = new WeatherForecastRequest<IEnumerable<Contracts.v2.WeatherForecast>>
        {
            Unit = unit
        };

        var result = await _mediator.Send(request);        
        return new OkObjectResult(result);
    }
}
