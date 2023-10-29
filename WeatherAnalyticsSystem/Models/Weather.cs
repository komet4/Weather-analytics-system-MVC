using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WeatherAnalyticsSystem.Models;

public class Weather
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string City { get; set; }
    
    [JsonPropertyName("sys")]
    public Sys Sys { get; set; }
    
    [JsonPropertyName("coord")]
    public Coord Coord { get; set; }
    
    [JsonPropertyName("main")]
    public Main Main { get; set; }
    
    [JsonPropertyName("dt")]
    public int DateTime { get; set; }
    
    [Column(TypeName = "timestamp")]
    public DateTime Timestamp { get; set; }
}