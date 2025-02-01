using Shop.Web.Models;

namespace Shop.Web.Service.Interfaces
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUSerIDAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);

        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
    

        Task<ResponseDto?> ApplyCouponAsync(CartDto Cart);
       

    }
}
