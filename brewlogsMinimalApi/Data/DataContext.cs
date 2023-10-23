using brewlogsMinimalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace brewlogsMinimalApi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Brewlog> Brewlog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "server=localhost;database=brewlogsdb;user=root;password=panama";

            var serverVersion = new MySqlServerVersion(new Version(8, 1, 0));

            optionsBuilder.UseMySql(connectionString, serverVersion);
        }

        /*
        public DataContext(DbContextOptions<DataContext> options, DbSet<Brewlog> brewlog) : base(options)
        {
            Brewlog = brewlog;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Entity Mappings
            modelBuilder.Entity<Brewlog>(entity => { entity.HasKey(e => e.Id); });
        }
    */
    }
}