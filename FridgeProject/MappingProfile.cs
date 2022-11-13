using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace FridgeProject
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}