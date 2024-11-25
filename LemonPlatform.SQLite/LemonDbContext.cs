using LemonPlatform.SQLite.Models;
using Microsoft.EntityFrameworkCore;

namespace LemonPlatform.SQLite
{
    public class LemonDbContext : DbContext
    {
        public LemonDbContext(DbContextOptions<LemonDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}