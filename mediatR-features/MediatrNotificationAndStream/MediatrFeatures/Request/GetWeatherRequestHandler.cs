using MediatR;

namespace MediatrFeatures.Request;

public class GetWeatherRequestHandler(GetWeatherService getWeatherService)
    : IRequestHandler<GetWeatherRequest, IEnumerable<WeatherForecast>>
{
    
    public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherRequest request, CancellationToken cancellationToken)
    {
       var summaries= await getWeatherService.GetAllSummaries(cancellationToken);
       return summaries;
    }
}