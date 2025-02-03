using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;

namespace Shop.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;   
        }
        public async Task<ResponseDto?> GetCartByUSerIDAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Url = Details.ShoppingCartApi + $"/api/CartApi/GetCart/"+userId

            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = cartDto,
                Url = Details.ShoppingCartApi + $"/api/CartApi/CartUpsert"

            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data=cartDetailsId,
                Url = Details.ShoppingCartApi + $"/api/CartApi/RemoveCart/" 

            });
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto Cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data=Cart,
                Url = Details.ShoppingCartApi + $"/api/CartApi/ApplyCoupon"

            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = cartDto,
                Url = Details.ShoppingCartApi + $"/api/CartApi/EmailCartRequst"

            });
        }
    }
}
