using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using System.Diagnostics;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IProductService _ProductService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService ProductService, ICartService cartService)
        {
            _logger = logger;
            _ProductService = ProductService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto>? ProductList = new();
            ResponseDto? response = await _ProductService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                ProductList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));

            }
            else
            {
                TempData["error"] = response?.Message;

            }
            return View(ProductList);
        }
        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto? product = new();
            ResponseDto? response = await _ProductService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

            }
            else
            {
                TempData["error"] = response?.Message;

            }
            return View(product);
        }
        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                },

            };
            CartDetailDto cartDetails = new CartDetailDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId,
            };
            List<CartDetailDto> cartDetailDtos = new List<CartDetailDto>() { cartDetails };
            cartDto.CartDetails = cartDetailDtos;
            ResponseDto? response = await _cartService.UpsertCartAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["Successful"] = "Item has been added to the Shopping Cart";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["error"] = response?.Message;

            }
            return View(productDto);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
