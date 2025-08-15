using Acme.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acme.Shop.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
