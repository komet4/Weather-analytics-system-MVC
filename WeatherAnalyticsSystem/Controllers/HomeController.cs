using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAnalyticsSystem.Data;
using WeatherAnalyticsSystem.Models;
using WeatherAnalyticsSystem.ViewModels;

namespace WeatherAnalyticsSystem.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;

    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Log()
    {
        var viewModel = new WeatherViewModel();
        viewModel.weathersList = await GetWeathersAsync(); //Все измерения для лога погоды
        return View(viewModel);
    }

    public async Task<IActionResult> Filter()
    {
        var viewModel = new WeatherViewModel();
        viewModel.weathersList = await GetWeathersAsync(); //Все измерения для лога погоды
        viewModel.uniqueCities = await UniqueCities(); //Уникальные города
        viewModel.AvgTemp = await AvgTempAsync(viewModel.uniqueCities, UnixTimeOfLastHour()); //Среднее значение температур за последний час
        return View(viewModel);
    }

    public async Task<IActionResult> TestChart()
    {
        var viewModel = new WeatherViewModel();
        viewModel.uniqueCities = await UniqueCities(); //Уникальные города
        viewModel.CitiesTempTime = await GetTempOfCurrentDayAsync(viewModel.uniqueCities, UnixTimeOfCurrentDay()); //Температура и время замера за текущей день
        return View(viewModel);
    }
    
    //Получение уникальных городов из бд
    public async Task<List<string>> UniqueCities()
    {
        return await _dbContext.Weathers.Select(w => w.City).Distinct().ToListAsync();
    }

    //Получение словаря уникальных городов со значением всех температур и время замера из бд
    public async Task<Dictionary<string, List<Tuple<double, int>>>> GetTempOfCityAsync(List<string> uniqueCities)
    {
        var result = new Dictionary<string, List<Tuple<double, int>>>();
        foreach (var city in uniqueCities)
        {
            var tempCity = await _dbContext.Weathers
                .Include(w => w.Main)
                .Where(w => w.City == city)
                .OrderBy(w => w.DateTime)
                .ToListAsync();

            if (tempCity.Count > 0)
            {
                var currentDayTemp = new List<Tuple<double, int>>();
                foreach (var w in tempCity)
                {
                    currentDayTemp.Add(Tuple.Create(w.Main.Temp, w.DateTime));
                }

                result.Add(city, currentDayTemp);
            }
        }
        return result;
    }

    //Получение словаря уникальных городов со значением всех температур и время замера за последние сутки из бд
    public async Task<Dictionary<string, List<Tuple<double, int>>>> GetTempOfCurrentDayAsync(List<string> uniqueCities,int unixTimeCurrentDay)
    {
        var result = new Dictionary<string, List<Tuple<double, int>>>();
        foreach (var city in uniqueCities)
        {
            var currentDayWeather = await _dbContext.Weathers
                .Include(w => w.Main)
                .Where(w => w.DateTime >= unixTimeCurrentDay && w.City == city)
                .OrderBy(w => w.DateTime)
                .ToListAsync();

            if (currentDayWeather.Count > 0)
            {
                var currentDayTemp = new List<Tuple<double, int>>();
                foreach (var w in currentDayWeather)
                {
                    currentDayTemp.Add(Tuple.Create(w.Main.Temp, w.DateTime));
                }
                result.Add(city, currentDayTemp);
            }
        }
        return result;
    }

    //Получение всех данных из бд
    public async Task<List<Weather>> GetWeathersAsync()
    {
        return await _dbContext.Weathers
            .Include(w => w.Main)
            .Include(w => w.Coord)
            .Include(w => w.Sys)
            .OrderByDescending(w => w.Id)
            .ToListAsync();
    }
    
    //Получение всех данных за последние сутки из бд
    public async Task<List<Weather>> GetWeatherOfCurrentDayAsync(int unixTimeCurrentDay)
    {
        return await _dbContext.Weathers
            .Include(w => w.Main)
            .Where(w => w.DateTime >= unixTimeCurrentDay)
            .OrderByDescending(w => w.Id)
            .ToListAsync();
    }

    //Получение словаря уникальных городов со средним значением температур за последний час без последнего замера температуры из бд
    public async Task<Dictionary<string,double>> AvgTempAsync(List<string> uniqueCities, int unixTimeLastHour)
    {
        var result = new Dictionary<string, double>();
        foreach (var city in uniqueCities)
        {
            var lastHourTemp = await _dbContext.Weathers
                .Where(w => w.DateTime >= unixTimeLastHour && w.City == city)
                .OrderByDescending(w => w.Id)
                .Skip(1)
                .Select(w => w.Main.Temp)
                .ToListAsync();
            if (lastHourTemp.Count > 0)
            {
                result.Add(city, lastHourTemp.Average());
            }
        }
        return result;
    }
    
    //Получение количество секунд unix времени на час меньше текущего
    public int UnixTimeOfLastHour()
    {
        var unixTime = (int)(DateTime.UtcNow.AddHours(-1) - new DateTime(1970, 1, 1)).TotalSeconds;
        return unixTime;
    }
    
    //Получение количество секунд unix времени до начала текущего дня
    public int UnixTimeOfCurrentDay()
    {
        var currentTime = DateTime.UtcNow;
        var startDateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0, DateTimeKind.Utc);
        var unixTime = (int)(startDateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        return unixTime;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}