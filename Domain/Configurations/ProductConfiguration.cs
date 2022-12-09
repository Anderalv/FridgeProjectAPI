using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Enums.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
       
        internal Product Product1 = new Product
        {
            Id = 1,
            Name = "Milk",
            DefaultQuantity = 1
        };
        internal Product Product2 = new Product
        {
            Id = 2,
            Name = "Bacon",
            DefaultQuantity = 2
        };
        internal Product Product3 = new Product
        {
            Id = 3,
            Name = "Beans",
            DefaultQuantity = 3
        };

        internal Product Product4 = new Product
        {
            Id = 4,
            Name = "Carrot",
            DefaultQuantity = 4
        };

        internal Product Product5 = new Product
        {
            Id = 5,
            Name = "Apple",
            DefaultQuantity = 5
        };

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(Product1, Product2, Product3, Product4, Product5);
        }
    }
}