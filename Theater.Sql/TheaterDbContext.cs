using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Theater.Entities.Authorization;

namespace Theater.Sql
{
    public class TheaterDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public TheaterDbContext(DbContextOptions<TheaterDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(TheaterDbContext))!);
        }
    }
}
