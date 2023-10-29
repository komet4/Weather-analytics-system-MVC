using WeatherAnalyticsSystem.Data;

namespace WeatherAnalyticsSystem.Services;

public class WeatherBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<string> _cities = new List<string>
    {
        "Rio de Janeiro", "Vancouver", "New York", "Sydney","Beijing",
        "Tokyo", "New Delhi","Moscow", "Berlin", "London"
    };

    public WeatherBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                foreach (var city in _cities)
                {
                    var result = await weatherService.GetWeatherByCityNameAsync(city);
                    //Запись данных из API, при условии, что таких записей нет в бд
                    if (!dbContext.Weathers.Any(w => w.DateTime == result.DateTime && w.City == result.City))
                    {
                        result.Timestamp = DateTime.Now;
                        dbContext.Weathers.Add(result);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}