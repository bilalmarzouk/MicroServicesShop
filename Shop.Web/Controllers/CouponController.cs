using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.Interfaces;

namespace Shop.Web.Controllers
{
    public class CouponController : Controller
    {
        private ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponAction()
        {
            List<CouponDto>? couponList = new();
            ResponseDto? response = await _couponService.GetAllCouponAsync();

            if (response != null && response.IsSuccess)
            {
                couponList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            return View();
        }
    }
}
