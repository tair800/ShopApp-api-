using AutoMapper;
using ShopApp_API_.Apps.AdminApp.Dtos.CategoryDto;
using ShopApp_API_.Apps.AdminApp.Dtos.ProductDto;
using ShopApp_API_.Entities;

namespace ShopApp_API_.Profiles
{
    public class MapperProfile : Profile
    {
        private readonly IHttpContextAccessor _contextAccessor;



        public MapperProfile(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            var request = _contextAccessor.HttpContext.Request;
            var urlBuilder = new UriBuilder(
                request.Scheme,
                request.Host.Host,
                request.Host.Port.Value
                );
            var url = urlBuilder.Uri.AbsoluteUri;


            CreateMap<Category, CategoryReturnDto>()
                .ForMember(destination => destination.Image, map => map.MapFrom(source => url + "/images/" + source.Image));

            CreateMap<Product, ProductReturnDto>();
            CreateMap<Category, CategoryInProductDto>();

        }
    }
}
