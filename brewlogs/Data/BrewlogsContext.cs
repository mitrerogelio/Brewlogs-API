using Microsoft.EntityFrameworkCore;
using brewlogs.Models;

namespace brewlogs.Data
{
    public class BrewlogsContext : DbContext
    {
        public BrewlogsContext(DbContextOptions<BrewlogsContext> options) : base(options) {
        }

        public DbSet<Brewlog>? Brewlogs { get; set; }

        // You can use OnModelCreating for fluent API configurations if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add fluent API configurations if necessary
        }
    }
}
