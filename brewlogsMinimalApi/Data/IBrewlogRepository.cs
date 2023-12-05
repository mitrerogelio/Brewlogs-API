using brewlogsMinimalApi.Model;

namespace brewlogsMinimalApi.Data;

public interface IBrewlogRepository
{
    public Task<int> SaveChangesAsync();
    public void AddEntity<T>(T entity);
    public void RemoveEntity<T>(T entity);
    public Task<List<Brewlog?>> GetBrewlogs();
    public Task<Brewlog?> GetBrewlog(Guid id);
}