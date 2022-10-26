using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class FridgeProductConfiguration : IEntityTypeConfiguration<FridgeProduct>
    {

        internal FridgeProduct FridgeProduct1 = new FridgeProduct
        {
            Id = 1,
            IdProduct = 1,
            IdFridge = 1,
            Quantity = 1
        };
        internal FridgeProduct FridgeProduct2 = new FridgeProduct
        {
            Id = 2,
            IdProduct = 2,
            IdFridge = 1,
            Quantity = 2
        };
        internal FridgeProduct FridgeProduct3 = new FridgeProduct
        {
            Id = 3,
            IdProduct = 3,
            IdFridge = 1,
            Quantity = 3
        };
        internal FridgeProduct FridgeProduct4 = new FridgeProduct
        {
            Id = 4,
            IdProduct = 4,
            IdFridge = 1,
            Quantity = 4
        };
        internal FridgeProduct FridgeProduct5 = new FridgeProduct
        {
            Id = 5,
            IdProduct = 5,
            IdFridge = 1,
            Quantity = 5
        };
        
        internal FridgeProduct FridgeProduct6 = new FridgeProduct
        {
            Id = 6,
            IdProduct = 2,
            IdFridge = 2,
            Quantity = 2
        };
        internal FridgeProduct FridgeProduct7 = new FridgeProduct
        {
            Id = 7,
            IdProduct = 3,
            IdFridge = 2,
            Quantity = 3
        };
        internal FridgeProduct FridgeProduct8 = new FridgeProduct
        {
            Id = 8,
            IdProduct = 4,
            IdFridge = 2,
            Quantity = 4
        };
        internal FridgeProduct FridgeProduct9 = new FridgeProduct
        {
            Id = 9,
            IdProduct = 5,
            IdFridge = 2,
            Quantity = 5
        };
        
        internal FridgeProduct FridgeProduct10 = new FridgeProduct
        {
            Id = 10,
            IdProduct = 3,
            IdFridge = 3,
            Quantity = 5
        };
        internal FridgeProduct FridgeProduct11 = new FridgeProduct
        {
            Id = 11,
            IdProduct = 4,
            IdFridge = 3,
            Quantity = 5
        };
        internal FridgeProduct FridgeProduct12 = new FridgeProduct
        {
            Id = 12,
            IdProduct = 5,
            IdFridge = 3,
            Quantity = 5
        };
        
        internal FridgeProduct FridgeProduct13 = new FridgeProduct
        {
            Id = 13,
            IdProduct = 4,
            IdFridge = 4,
            Quantity = 4
        };
        internal FridgeProduct FridgeProduct14 = new FridgeProduct
        {
            Id = 14,
            IdProduct = 5,
            IdFridge = 4,
            Quantity = 4
        };


        public void Configure(EntityTypeBuilder<FridgeProduct> builder)
        {
            builder.HasData(FridgeProduct1, FridgeProduct2, FridgeProduct3, FridgeProduct4, FridgeProduct5, FridgeProduct6,
                FridgeProduct7, FridgeProduct8, FridgeProduct9, FridgeProduct10, FridgeProduct11,
                FridgeProduct12, FridgeProduct13, FridgeProduct14);
        }
    }
}