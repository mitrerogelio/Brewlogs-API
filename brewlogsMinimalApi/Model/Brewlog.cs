using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace brewlogsMinimalApi.Model;

public class Brewlog
{
    [Key]
    public int Id { get; set; }
    public string? CoffeeName { get; set; }
    public int Dose { get; set; }
    public string? Grind { get; set; }
    public int BrewRatio { get; set; }
    public string? Roast { get; set; }
    public string? BrewerUsed { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [JsonIgnore]
    public DateTime? LastUpdated { get; set; }
}

