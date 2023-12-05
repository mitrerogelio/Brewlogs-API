using brewlogsMinimalApi.Model;

namespace brewlogsMinimalApi.Data;

public interface IBrewlogRepository
{
    public bool SaveChanges();
    public void AddEntity<T>(T entityToAdd);
    public void RemoveEntity<T>(T entityToAdd);
    public IEnumerable<Brewlog> GetBrewlogs();
    public Brewlog GetBrewlog();
}