using AutoMapper;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;

namespace FridgeProject
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Fridge, FridgeDto>();
            
            CreateMap<Product, ProductInFridgeDto>();
            CreateMap<ProductForCreationDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}