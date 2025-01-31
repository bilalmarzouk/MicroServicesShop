using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Shop.Services.ProductAPI.Models;
using Shop.Services.ProductAPI.Models.Dto;

namespace Shop.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(conifg =>
            {
                conifg.CreateMap<Product, ProductDto>();
                conifg.CreateMap<ProductDto, Product>();
            });
        }
    }

}




