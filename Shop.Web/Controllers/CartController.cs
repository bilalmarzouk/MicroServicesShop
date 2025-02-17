using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;
using System.IdentityModel.Tokens.Jwt;

namespace Shop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartForLoggedInUser());
        }
        [Authorize]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartForLoggedInUser());
        }
        [Authorize]
        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart =await LoadCartForLoggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.FirstName = cartDto.CartHeader.FirstName;
            cart.CartHeader.LastName = cartDto.CartHeader.LastName;

            var response = await _orderService.CreateOrderAsync(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            if(response != null && response.IsSuccess)
            {
                StripeRequestDto stripeRequestDto = new()
                {
                    ApproveUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CancelUrl = domain + "cart/checkout",
                    OrderHeader = orderHeaderDto
                };
                var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
                StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(stripeResponse.Result));
                Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                return new StatusCodeResult(303);
            }
            return View();

        }
     
        public async Task<IActionResult> Confirmation(int orderId)
        {
            

            ResponseDto? response = await _orderService.ValidateStripeSession(orderId);

            if (response != null && response.IsSuccess)
            {
                OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
                if(orderHeader.Status == Details.Status_Approved)
                {
                    return View(orderId);
                }
            }
            //todo redirect to error page based on status
            return View(orderId);
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
