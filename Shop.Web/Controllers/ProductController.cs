using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.Interfaces;

namespace Shop.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _ProductService;
        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }
        public async Task<IActionResult> ProductIndex()
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
        public async Task<IActionResult> ProductCreate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto Product)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _ProductService.CreateProductAsync(Product);
                if (response != null && response.IsSuccess)
                {
                    TempData["Successful"] = "Product created Successfuly";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;

                }
            }
            return View(Product);
        }

        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _ProductService.GetProductByIdAsync(ProductId);
                if (response != null && response.IsSuccess)
                {
                    ProductDto? Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                    return View(Product);
                }
                else
                {
                    TempData["error"] = response?.Message;

                }
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto Product)
        {
            ResponseDto? response = await _ProductService.DeleteProductByIdAsync(Product.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["Successful"] = "Product deleted Successfuly";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;

            }

            return View(Product);

        }

        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _ProductService.GetProductByIdAsync(ProductId);
                if (response != null && response.IsSuccess)
                {
                    ProductDto? Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                    return View(Product);
                }
                else
                {
                    TempData["error"] = response?.Message;

                }
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto Product)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _ProductService.UpdateProductByIdAsync(Product);
                if (response != null && response.IsSuccess)
                {
                    TempData["Successful"] = "Product updated Successfuly";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;

                }
            }
            return View(Product);

        }
    }
}
