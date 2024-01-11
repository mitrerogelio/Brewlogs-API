namespace BrewlogsApi.Dtos;

public class BrewlogDto
{
    public int? Id { get; set; } = null;
    public string CoffeeName { get; set; } = "";
    public int Dose { get; set; }
    public string Grind { get; set; } = "";
    public int BrewRatio { get; set; }
    public string Roast { get; set; } = "";
    public string BrewerUsed { get; set; } = "";
}