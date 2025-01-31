using Shop.Services.ShoppingCart.Model.Dto;

namespace Shop.Services.ShoppingCart.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
