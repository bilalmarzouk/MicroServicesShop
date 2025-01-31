using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Shop.Services.ShoppingCart.Model;
using Shop.Services.ShoppingCart.Model.Dto;


namespace Shop.Services.ShoppingCart
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(conifg =>
            {
                conifg.CreateMap<CartDetails, CartDetailDto>();
                conifg.CreateMap<CartDetailDto, CartDetails>();
                conifg.CreateMap<CartHeader, CartHeaderDto>();
                conifg.CreateMap<CartHeaderDto, CartHeader>();
            });
        }
    }

}




