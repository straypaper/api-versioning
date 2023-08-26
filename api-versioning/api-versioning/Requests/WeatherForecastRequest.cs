using api_versioning.Controllers;
using MediatR;

namespace api_versioning.Requests;

public class WeatherForecastRequest<TResponse> : IRequest<TResponse>
{
    public TemperatureUnit Unit { get; set; }
}