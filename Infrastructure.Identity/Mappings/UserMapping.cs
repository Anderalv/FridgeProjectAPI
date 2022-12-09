using Application.DTOs.Account;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Identity.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}