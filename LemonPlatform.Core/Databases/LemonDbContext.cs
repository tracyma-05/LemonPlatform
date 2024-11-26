using LemonPlatform.Core.Databases.Models;
using LemonPlatform.Core.Databases.Quartz;
using Microsoft.EntityFrameworkCore;

namespace LemonPlatform.Core.Databases
{
    public class LemonDbContext : DbContext
    {
        public LemonDbContext()
        { }

        public LemonDbContext(DbContextOptions<LemonDbContext> options)
            : base(options)
        { }

        public DbSet<UserPreference> UserPreference { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPreference>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.ModuleName).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.LastModifiedAt).IsRequired();
            });

            modelBuilder.AddQuartz(builder => builder.UseSqlite());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=lemon.db");
        }
    }
}