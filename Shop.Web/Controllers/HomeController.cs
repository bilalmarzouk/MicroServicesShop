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

        public HomeController(ILogger<HomeController> logger, IProductService ProductService)
        {
            _logger = logger;
            _ProductService = ProductService;
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
