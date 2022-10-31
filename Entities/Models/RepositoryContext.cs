using Entities.Configuration;
using Microsoft.EntityFrameworkCore;


namespace Entities.Models
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new FridgeConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeProductConfiguration()); 
            modelBuilder.ApplyConfiguration(new ModelConfiguration()); 
            modelBuilder.ApplyConfiguration(new ProductConfiguration()); 
        }
        
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FridgeProduct> FridgeProducts { get; set; }
        
        
      
    }
}