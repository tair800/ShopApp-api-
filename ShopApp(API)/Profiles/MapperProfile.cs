using AutoMapper;
using ShopApp_API_.Apps.AdminApp.Dtos.CategoryDto;
using ShopApp_API_.Apps.AdminApp.Dtos.ProductDto;
using ShopApp_API_.Entities;

namespace ShopApp_API_.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(destination => destination.Image, map => map.MapFrom(source => "http://localhost:5036/images/" + source.Image)).ReverseMap();

            CreateMap<Product, ProductReturnDto>();
            CreateMap<Category, CategoryInProductDto>();

        }
    }
}
