using Microsoft.EntityFrameworkCore;
using ShopApp_API_.Entities;
using System.Reflection;

namespace ShopApp_API_.Data
{
    public class ShopAppDbContext : DbContext
    {
        public ShopAppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
