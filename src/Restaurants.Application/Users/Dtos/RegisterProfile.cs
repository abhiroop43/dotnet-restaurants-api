using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Dtos;

public class RegisterProfile : Profile
{
    public RegisterProfile()
    {
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality));
    }
}