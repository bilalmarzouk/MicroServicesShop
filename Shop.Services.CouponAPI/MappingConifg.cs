using AutoMapper;
using Shop.Services.CouponAPI.Models;
using Shop.Services.CouponAPI.Models.Dto;

namespace Shop.Services.CouponAPI
{
    public class MappingConifg
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConifg = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
            });
            return mappingConifg;
        }
    }
}
