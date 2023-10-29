using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WeatherAnalyticsSystem.Models;

public class Coord
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
    public int Id { get; set; }
    
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
}