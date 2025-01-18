using System.Net;
using Microsoft.Extensions.Caching.Hybrid;

namespace HybridCacheDemo;

/// <summary>
/// Используя HybridCache - встроена защита от перегрузки (когда одновроемено 2 потока обращаются к кешу с одним ключем)
/// HybridCache - по умолчанию работает как IMemoryCache но с защитой от перегрузки
/// Чтобы подключить кэширование Redis просто добавить "builder.Services.AddStackExchangeRedisCache"
/// </summary>
/// <param name="httpClientFactory"></param>
/// <param name="hybridCache"></param>
public class WeatherService(
	IHttpClientFactory httpClientFactory,
	HybridCache hybridCache
	)
{
	private const string ApiKey= "851703e3a473ba408443f63d0693ebfe";
	
	public async Task<WeatherResponse?> GetCurrentWeatherHybridCacheAsync(string city)
	{
		var cacheKey = $"weather:{city}";
		var weather = await hybridCache.GetOrCreateAsync<WeatherResponse?>(cacheKey,
		 async _ => await GetWeatherAsync(city),
		 new HybridCacheEntryOptions()
		 {
			 Expiration = TimeSpan.FromSeconds(5), //Время жизни в кеше памяти
			 LocalCacheExpiration = TimeSpan.FromSeconds(5), //Время жизни в кеше Redis
			 //Flags = HybridCacheEntryFlags.DisableDistributedCache //Отключить сохранение в redis
		 },
		 tags:["weather"]);// Можно удалять данные из кэша по тэгу hybridCache.RemoveByTagAsync("weather");

		 return weather;
	}
	
	private async Task<WeatherResponse?> GetWeatherAsync(string city)
	{
		var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}";
		var httpClient = httpClientFactory.CreateClient();
		var weatherResponse = await httpClient.GetAsync(url);
		if (weatherResponse.StatusCode == HttpStatusCode.NotFound)
		{
			return null;
		}
		return await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
	}
}

