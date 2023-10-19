namespace Weather_analytics_system.Models;

public class Measurement
{
    public int Id { get; set; }
    public string City { get; set; }
    public double Temp_now { get; set; }
    public double Temp_min { get; set; }
    public double Temp_max { get; set; }
    public DateTime DateTime { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Country { get; set; }
}