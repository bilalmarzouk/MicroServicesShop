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
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? couponList = new();
            ResponseDto? response = await _couponService.GetAllCouponAsync();

            if (response != null && response.IsSuccess)
            {
                couponList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
                
            }
            else
            {
                TempData["error"] = response?.Message;

            }
            return View(couponList);
        }
        public async Task<IActionResult> CouponCreate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto coupon)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(coupon);
                if (response != null && response.IsSuccess)
                {
                    TempData["Successful"] = "Coupon created Successfuly";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;

                }
            }
            return View(coupon);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);
                if (response != null && response.IsSuccess)
                {
                    CouponDto? coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

                    return View(coupon);
                }
                else
                {
                    TempData["error"] = response?.Message;

                }
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto coupon)
        {
            ResponseDto? response = await _couponService.DeleteCouponByIdAsync(coupon.CouponId);
            if (response != null && response.IsSuccess)
            {
                TempData["Successful"] = "Coupon deleted Successfuly";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;

            }

            return View(coupon);

        }
    }
}
