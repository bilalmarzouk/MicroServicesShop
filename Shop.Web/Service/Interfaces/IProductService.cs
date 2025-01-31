using Shop.Web.Models;

namespace Shop.Web.Service.Interfaces
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductAsync(string code);
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> CreateProductAsync(ProductDto Productdto);

        Task<ResponseDto?> UpdateProductByIdAsync(ProductDto Product);
        Task<ResponseDto?> DeleteProductByIdAsync(int id);
    }
}
