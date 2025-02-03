using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Shop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartForLoggedInUser());
        }

        private async Task<CartDto> LoadCartForLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            ResponseDto cart = await _cartService.GetCartByUSerIDAsync(userId);

            if(cart != null && cart.IsSuccess)
            {
               var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(cart.Result));
                return cartDto;
            }
            return new();
        }
        public async Task<IActionResult> Remove(int CartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            ResponseDto? cart = await _cartService.RemoveFromCartAsync(CartDetailsId);

            if (cart != null && cart.IsSuccess)
            {
                TempData["Successful"] = "Cart Updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = cart.Message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            ResponseDto? cart = await _cartService.ApplyCouponAsync(cartDto);

            if (cart != null && cart.IsSuccess)
            {
                TempData["Successful"] = "Cart Updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";

            ResponseDto? cart = await _cartService.ApplyCouponAsync(cartDto);

            if (cart != null && cart.IsSuccess)
            {
                TempData["Successful"] = "Cart Updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {

            CartDto cart = await LoadCartForLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.EmailCart(cart);

            if (response != null && response.IsSuccess)
            {
                TempData["Successful"] = "Email will be processed and sent shortly.";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = "Error, Please try again.";
            return RedirectToAction(nameof(CartIndex));
        }
    }
}
