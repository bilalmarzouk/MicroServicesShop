using Shop.Web.Models;

namespace Shop.Web.Service.Interfaces
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string code);
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?>CreateCouponAsync(CouponDto coupondto);

        Task<ResponseDto?> UpdateCouponByIdAsync(CouponDto coupon);
        Task<ResponseDto?> DeleteCouponByIdAsync(int id);

    }
}
