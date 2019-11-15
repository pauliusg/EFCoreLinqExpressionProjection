using Microsoft.EntityFrameworkCore;

namespace EFCoreLinqExpressionProjection.Test.Model
{
    internal sealed class ProjectsDbContext : DbContext
    {
        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Subproject> Subprojects { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
