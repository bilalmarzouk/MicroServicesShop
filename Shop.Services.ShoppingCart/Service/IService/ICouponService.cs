using Shop.Services.ShoppingCart.Model.Dto;

namespace Shop.Services.ShoppingCart.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCod);
    }
}
