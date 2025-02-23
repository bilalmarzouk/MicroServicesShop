using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;

namespace Shop.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductAsync(ProductDto Productdto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = Productdto,
                Url = Details.ProductAPIBase + $"/api/ProductApi/",
                ContentType = Details.ContentType.MultipartFromData

            });
        }

        public async Task<ResponseDto?> DeleteProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.DELETE,
                Url = Details.ProductAPIBase + $"/api/ProductApi/{id}"

            });
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Url = Details.ProductAPIBase + "/api/ProductApi"

            });
        }

        public async Task<ResponseDto?> GetProductAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Url = Details.ProductAPIBase + $"/api/ProductApi/GetByCode/{code}"

            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Url = Details.ProductAPIBase + $"/api/ProductApi/{id}"

            });
        }

        public async Task<ResponseDto?> UpdateProductByIdAsync(ProductDto Product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.PUT,
                Data = Product,
                Url = Details.ProductAPIBase + $"/api/ProductApi/",
                ContentType = Details.ContentType.MultipartFromData

            });
        }

    }
}
