using System.ComponentModel.DataAnnotations;

namespace brewlogsMinimalApi.Model;

public class Brewlog
{
    [Key] public int Id { get; set; }
    public int Author { get; set; }
    public string CoffeeName { get; set; } = "";
    public int Dose { get; set; }
    public string Grind { get; set; } = "";
    public int BrewRatio { get; set; }
    public string Roast { get; set; } = "";
    public string BrewerUsed { get; set; } = "";
}