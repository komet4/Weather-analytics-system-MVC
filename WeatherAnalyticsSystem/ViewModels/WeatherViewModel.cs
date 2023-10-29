using WeatherAnalyticsSystem.Models;

namespace WeatherAnalyticsSystem.ViewModels;

public class WeatherViewModel
{
    public List<string> uniqueCities { get; set; }
    public List<Weather> weathersList { get; set; }
    public Dictionary<string,double> AvgTemp { get; set; }
    public Dictionary<string, List<Tuple<double, int>>> CitiesTempTime { get; set; }
    //Конвертирование unix секунд из int в DateTime, для использования в представлении
    public DateTime ConvertUnixTimeToDateTime(int unixTime) 
        => DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
}