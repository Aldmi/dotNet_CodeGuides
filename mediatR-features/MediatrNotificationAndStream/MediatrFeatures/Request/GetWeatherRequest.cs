using MediatR;

namespace MediatrFeatures.Request;

public class GetWeatherRequest : IRequest<IEnumerable<WeatherForecast>>
{
    public required string City { get; init; }
}