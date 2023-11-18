using brewlogsMinimalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace brewlogsMinimalApi.Data
{
    public class BrewlogsDbContext : DbContext
    {
        public BrewlogsDbContext(DbContextOptions<BrewlogsDbContext> options)
            : base(options) { }

        public DbSet<Brewlog> Brewlogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brewlog>().HasData(
                new Brewlog
                {
                    Id = 1,
                    CoffeeName = "Mugshots Chiapas",
                    Dose = 20,
                    Grind = "Medium",
                    BrewRatio = 15,
                    Roast = "Medium",
                    BrewerUsed = "AeroPress",
                },
                new Brewlog
                {
                    Id = 2,
                    CoffeeName = "Caribou Light Roast",
                    Dose = 18,
                    Grind = "Fine",
                    BrewRatio = 16,
                    Roast = "Light",
                    BrewerUsed = "Pour-Over",
                },
                new Brewlog
                {
                    Id = 3,
                    CoffeeName = "Five Watt Guatemala",
                    Dose = 18,
                    Grind = "Fine",
                    BrewRatio = 16,
                    Roast = "Light",
                    BrewerUsed = "Pour-Over",
                }
            );
        }
    }
}