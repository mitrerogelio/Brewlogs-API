using brewlogsMinimalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace brewlogsMinimalApi.Data;

public class BrewlogsDbContext : DbContext
{
    public IHttpContextAccessor ContextAccessor { get; }

    public BrewlogsDbContext(DbContextOptions<BrewlogsDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        ContextAccessor = httpContextAccessor;
    }

    public required DbSet<Brewlog> Brewlogs { get; set; }
}