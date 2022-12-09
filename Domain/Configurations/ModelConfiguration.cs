using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Enums.Configurations
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        internal Model Model1 = new Model
        {
            Id = 1,
            Name = "Atlant",
            Year = 2001
        };
        internal Model Model2 = new Model
        {
            Id = 2,
            Name = "Vestfrost",
            Year = 2002
        };
        internal Model Model3 = new Model
        {
            Id = 3,
            Name = "Mitsubishi",
            Year = 2003
        };
        internal Model Model4 = new Model
        {
            Id = 4,
            Name = "Bosch",
            Year = 2004
        };
        internal Model Model5 = new Model
        {
            Id = 5,
            Name = "Samsung",
            Year = 2005
        };

        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasData(Model1, Model2, Model3, Model4, Model5);
        }
    }
}