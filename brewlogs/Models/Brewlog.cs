using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace brewlogs.Models;

public class Brewlog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? CoffeeName { get; set; }
    public int Dose { get; set; }
    public string? Grind { get; set; }
    public int BrewRatio { get; set; }
    public string? Roast { get; set; }
    public string? BrewerUsed { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime LastUpdated { get; set; }
}