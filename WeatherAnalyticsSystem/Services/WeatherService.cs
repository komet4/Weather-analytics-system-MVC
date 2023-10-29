using WeatherAnalyticsSystem.Models;

namespace WeatherAnalyticsSystem.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
 
    public WeatherService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<Weather> GetWeatherByCityNameAsync(string city)
    {
        var client = _httpClientFactory.CreateClient();
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid=371fc816a88fdc5758b1248fd1c95b89&units=metric";
        var response = await client.GetFromJsonAsync<Weather>(url);
        return response;
    }
}