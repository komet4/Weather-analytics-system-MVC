using WeatherAnalyticsSystem.Models;

namespace WeatherAnalyticsSystem.Services;

public interface IWeatherService
{
    Task<Weather?> GetWeatherByCityNameAsync(string city);
}