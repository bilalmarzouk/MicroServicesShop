using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Shop.Services.OrderApi.Models;
using Shop.Services.OrderApi.Models.Dto;



namespace Shop.Services.OrderApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(conifg =>
            {
                conifg.CreateMap<OrderHeaderDto, CartHeaderDto>().
                ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();
                conifg.CreateMap<CartDetailDto, OrderDetailsDto>().
                  ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                  .ForMember(dest => dest.ProductPrice, u => u.MapFrom(src => src.Product.Price));
                conifg.CreateMap<OrderDetailsDto, CartDetailDto>();
                conifg.CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
                conifg.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                conifg.CreateMap<OrderHeaderDto, OrderHeader>().ReverseMap();
            })
            {

            };
        }
    }

}




