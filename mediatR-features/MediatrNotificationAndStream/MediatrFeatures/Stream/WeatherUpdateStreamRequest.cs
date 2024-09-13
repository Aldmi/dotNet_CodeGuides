using MediatR;

namespace MediatrFeatures.Stream;

public class WeatherUpdateStreamRequest : IStreamRequest<WeatherForecast>
{
    public string City { get; }

    public WeatherUpdateStreamRequest(string city)
    {
        City = city;
    }
}