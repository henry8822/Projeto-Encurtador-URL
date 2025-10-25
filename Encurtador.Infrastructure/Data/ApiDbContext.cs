using Encurtador.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Encurtador.Infrastructure.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        public DbSet<ShortUrl> ShortUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortUrl>()
                .HasIndex(u => u.ShortCode)
                .IsUnique();

        }
    }
}