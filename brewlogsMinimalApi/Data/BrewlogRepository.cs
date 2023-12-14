using brewlogsMinimalApi.Model;

namespace brewlogsMinimalApi.Data;

public class BrewlogRepository : IBrewlogRepository
{
    private readonly DbContext _context;

    public BrewlogRepository(DbContext context)
    {
        _context = context;
    }

    /*
    public Task<int> SaveChangesAsync()
    {
    }

    public void AddEntity<T>(T entity)
    {
    }

    public void RemoveEntity<T>(T entity)
    {
    }

    public async Task<List<Brewlog>?> GetBrewlogs()
    {
    }

    public async Task<Brewlog?> GetBrewlog(Guid id)
    {
    }
*/
}