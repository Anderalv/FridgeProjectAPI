using Application.DTOs.Fridge;
using Application.DTOs.Model;
using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Fridge, FridgeDto>();
            CreateMap<Model, ModelDto>();
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