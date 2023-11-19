using System.Security.Claims;
using brewlogsMinimalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace brewlogsMinimalApi.Data
{
    public class BrewlogsDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BrewlogsDbContext(DbContextOptions<BrewlogsDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Brewlog> Brewlogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            modelBuilder.Entity<Brewlog>()
            .Property(b => b.Author)
            .HasDefaultValue(userId);
        }
    }
}