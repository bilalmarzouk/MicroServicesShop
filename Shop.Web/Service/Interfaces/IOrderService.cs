using Shop.Web.Models;

namespace Shop.Web.Service.Interfaces
{
    public interface IOrderService
    {
      
        Task<ResponseDto?>CreateOrderAsync(CartDto cartDto);
        Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);

        Task<ResponseDto?> ValidateStripeSession(int orderDetailId);
        Task<ResponseDto?> GetAllOrder(string? userId);
        Task<ResponseDto?> GetOrder(int orderId);
        Task<ResponseDto?> UpdateOrderStatus(int orderId,string newStatus);

    }
}
