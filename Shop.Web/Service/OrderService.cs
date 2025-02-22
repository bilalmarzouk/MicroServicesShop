using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;

namespace Shop.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;   
        }
        public async Task<ResponseDto?> CreateOrderAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = cartDto,
                Url = Details.OrderApiBase + $"/api/orderAPI/CreateOrder"

            });
        }

        public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = stripeRequestDto,
                Url = Details.OrderApiBase + $"/api/orderAPI/CreateStripeSession"

            });
        }

        public async Task<ResponseDto?> GetAllOrder(string? userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Data=userId,
                Url = Details.OrderApiBase + $"/api/orderAPI/GetOrders"

            });
        }

        public async  Task<ResponseDto?> GetOrder(int orderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.GET,
                Url = Details.OrderApiBase + $"/api/orderAPI/GetOrder/{orderId}"

            });
        }

        public async  Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = newStatus,
                Url = Details.OrderApiBase + $"/api/orderAPI/UpdateOrderStatus/{orderId}"

            });
        }

        public async Task<ResponseDto?> ValidateStripeSession(int orderDetailId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = orderDetailId,
                Url = Details.OrderApiBase + $"/api/orderAPI/ValidateStripeSession/"

            });
        }
    }
}
