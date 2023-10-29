using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WeatherAnalyticsSystem.Models;

public class Main
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
    public int Id { get; set; }
    
    [JsonPropertyName("temp")]
    public double Temp { get; set; }
    
    [JsonPropertyName("temp_min")]
    public double Temp_min { get; set; }
    
    [JsonPropertyName("temp_max")]
    public double Temp_max { get; set; }
}