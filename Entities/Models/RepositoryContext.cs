using Entities.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new FridgeConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeProductConfiguration()); 
            modelBuilder.ApplyConfiguration(new ModelConfiguration()); 
            modelBuilder.ApplyConfiguration(new ProductConfiguration()); 
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
        
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FridgeProduct> FridgeProducts { get; set; }
        
        
      
    }
}