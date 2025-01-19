using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;

namespace Shop.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;   
        }
        public async Task<ResponseDto?> CreateCouponAsync(CouponDto coupondto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = coupondto,
                Url = Details.CouponAPIBase + $"/api/CouponApi/"

            });
        }

        public async Task<ResponseDto?> DeleteCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.DELETE,
                Url = Details.CouponAPIBase + $"/api/CouponApi/{id}"

            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Url = Details.CouponAPIBase + "/api/CouponApi"

            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Url = Details.CouponAPIBase + $"/api/CouponApi/GetByCode/{code}"

            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Url = Details.CouponAPIBase + $"/api/CouponApi/{id}"

            });
        }

        public async Task<ResponseDto?> UpdateCouponByIdAsync(CouponDto coupon)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.PUT,
                Data = coupon,
                Url = Details.CouponAPIBase + $"/api/CouponApi/"

            });
        }
    }
}
