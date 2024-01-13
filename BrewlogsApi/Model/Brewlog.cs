using System.ComponentModel.DataAnnotations;

namespace BrewlogsApi.Model;

public class Brewlog
{
    [Key] public int Id { get; init; }
    public int Author { get; init; }
    public string CoffeeName { get; init; } = "";
    public int Dose { get; init; }
    public string Grind { get; init; } = "";
    public int BrewRatio { get; init; }
    public string Roast { get; init; } = "";
    public string BrewerUsed { get; init; } = "";
}