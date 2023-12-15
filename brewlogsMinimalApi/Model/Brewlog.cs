using System.ComponentModel.DataAnnotations;

namespace brewlogsMinimalApi.Model;

public class Brewlog
{
    [Key] public int Id { get; init; }
    public int Author { get; init; }
    public string CoffeeName { get; set; } = "";
    public int Dose { get; set; }
    public string Grind { get; set; } = "";
    public int BrewRatio { get; set; }
    public string Roast { get; set; } = "";
    public string BrewerUsed { get; set; } = "";
}