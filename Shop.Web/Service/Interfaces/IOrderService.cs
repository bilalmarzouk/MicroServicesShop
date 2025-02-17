using Shop.Web.Models;

namespace Shop.Web.Service.Interfaces
{
    public interface IOrderService
    {
      
        Task<ResponseDto?>CreateOrderAsync(CartDto cartDto);
        Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);

        Task<ResponseDto?> ValidateStripeSession(int orderDetailId);

    }
}
