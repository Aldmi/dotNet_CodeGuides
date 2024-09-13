using System.Runtime.CompilerServices;
using MediatR;

namespace MediatrFeatures.Stream;

public class WeatherUpdateStreamHandler(GetWeatherService getWeatherService) : IStreamRequestHandler<WeatherUpdateStreamRequest, WeatherForecast>
{
    public async IAsyncEnumerable<WeatherForecast> Handle(WeatherUpdateStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000, cancellationToken);
            yield return getWeatherService.GetRandomWeatherForecast();
        }
    }
}