using System.Text.RegularExpressions;
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
      CreateMap<CreateProductDto, Product>()
      .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => Regex.Replace(src.Name, @"\s+", "").ToLower()));
      CreateMap<UpdateProductDto, Product>();
      CreateMap<Cart, CartDto>();
    }
  }
}