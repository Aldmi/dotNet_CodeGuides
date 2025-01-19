namespace HybridCacheDemo;

/*
{
	"coord": {
		"lon": -0.1257,
		"lat": 51.5085
	},
	"weather": [
	{
		"id": 804,
		"main": "Clouds",
		"description": "overcast clouds",
		"icon": "04d"
	}
	],
	"base": "stations",
	"main": {
		"temp": 282.44,
		"feels_like": 282.17,
		"temp_min": 281.49,
		"temp_max": 283.23,
		"pressure": 1033,
		"humidity": 91,
		"sea_level": 1033,
		"grnd_level": 1029
	},
	"visibility": 10000,
	"wind": {
		"speed": 1.34,
		"deg": 215,
		"gust": 1.79
	},
	"clouds": {
		"all": 100
	},
	"dt": 1736956341,
	"sys": {
		"type": 2,
		"id": 2075535,
		"country": "GB",
		"sunrise": 1736927957,
		"sunset": 1736958010
	},
	"timezone": 0,
	"id": 2643743,
	"name": "London",
	"cod": 200
}
*/
public class WeatherResponse
{
	public int Timezone { get; set; }
	public int Id { get; set; }
	public string Name { get; set; }
	public int  Visibility { get; set; }
}
