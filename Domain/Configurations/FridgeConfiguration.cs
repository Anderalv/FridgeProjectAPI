using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Enums.Configurations
{
    public class FridgeConfiguration : IEntityTypeConfiguration<Fridge>
    {
        private static ModelConfiguration _modelConfiguration = new ModelConfiguration();


        internal Fridge Fridge1 = new Fridge
        {
            Id = 1,
            IdModel = 1,
            Name = "Fridge1",
            OwnerName = "Ivan"
        };
        internal Fridge Fridge2 = new Fridge
        {
            Id = 2,
            IdModel = 1,
            Name = "Fridge2",
            OwnerName = "Andrey"
        };
        internal Fridge Fridge3 = new Fridge
        {
            Id = 3,
            IdModel = 2,
            Name = "Fridge3",
            OwnerName = "Dima"
        };
        internal Fridge Fridge4 = new Fridge
        {
            Id = 4,
            IdModel = 3,
            Name = "Fridge4",
            OwnerName = "Vova"
        };
        internal Fridge Fridge5 = new Fridge
        {
            Id = 5,
            IdModel = 3,
            Name = "Fridge5",
            OwnerName = "Egor"
        };
        
        
        public void Configure(EntityTypeBuilder<Fridge> builder)
        {
            builder.HasData(Fridge1, Fridge2, Fridge3, Fridge4, Fridge5);
        }
    }
}