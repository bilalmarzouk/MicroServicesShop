using Shop.Services.OrderApi.Models.Dto;

namespace Shop.Services.OrderApi.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
