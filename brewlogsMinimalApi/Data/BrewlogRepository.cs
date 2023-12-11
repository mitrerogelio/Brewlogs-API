using brewlogsMinimalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace brewlogsMinimalApi.Data;

public class BrewlogRepository : IBrewlogRepository
{
    private readonly BrewlogsDbContext _context;

    public BrewlogRepository(BrewlogsDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void AddEntity<T>(T entity)
    {
        if (entity != null)
        {
            _context.Add(entity);
        }
    }

    public void RemoveEntity<T>(T entity)
    {
        if (entity != null)
        {
            _context.Remove(entity);
        }
    }

    public async Task<List<Brewlog>?> GetBrewlogs()
    {
        return await _context.Brewlogs.ToListAsync();
    }

    public async Task<Brewlog?> GetBrewlog(Guid id)
    {
        return await _context.Brewlogs.FindAsync(id);
    }
}