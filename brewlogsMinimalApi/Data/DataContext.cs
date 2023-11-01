using brewlogsMinimalApi.Model;
using Microsoft.EntityFrameworkCore;


namespace brewlogsMinimalApi.Data
{
  public class DataContext : DbContext
  {
    public DbSet<Brewlog>? Brewlogs { get; set; }

    public string DbPath { get; }

    public DataContext()
    {
      var folder = Environment.SpecialFolder.LocalApplicationData;
      var path = Environment.GetFolderPath(folder);
      DbPath = System.IO.Path.Join(path, "brewlogsdb");
    }

    // Configures EF to create a Sqlite database file in special "local" folder
    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
  }
}
