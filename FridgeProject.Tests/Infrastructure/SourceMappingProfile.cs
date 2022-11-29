using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace FridgeProject.Tests.Infrastructure
{
    public class SourceMappingProfile : Profile
    {
        public SourceMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Fridge, FridgeDto>();
            CreateMap<Model, ModelDto>();
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<FridgeProduct, ProductInFridgeDto>();
            CreateMap<ProductInFridgeDto, ProductInFridgeDtoToReturn>();
            CreateMap<FridgeProduct, FridgeProductDto>();
            CreateMap<EditFridgeDto, Fridge>();
            CreateMap<FridgeForCreationDto, Fridge>();
            
            CreateMap<ProductForCreateDto, ProductDto>().ForMember(c => c.DefaultQuantity,
                opt => opt.MapFrom(x => x.Quantity)); 
            
            CreateMap<ProductForUpdateDto, Product>().ForMember(c => c.DefaultQuantity,
                opt => opt.MapFrom(x => x.Quantity)); 
            
            CreateMap<ProductForCreateDto, Product>().ForMember(c => c.DefaultQuantity,
                opt => opt.MapFrom(x => x.Quantity));

            CreateMap<ProductForAddToFridgeDto, FridgeProduct>().ForMember(c => c.IdFridge,
                opt => opt.MapFrom(x => x.FridgeId));
        }
    }
}