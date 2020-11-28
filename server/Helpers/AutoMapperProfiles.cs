using AutoMapper;
using e_commerce.DTOs;
using e_commerce.Entities;

namespace e_commerce.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<User, UserDto>();
      CreateMap<RegisterDto, User>();
      CreateMap<Product, ProductDto>()
        .ForMember(dest => dest.CollectionName, opt => opt.MapFrom(src => src.Collection.Name));
    }
  }
}