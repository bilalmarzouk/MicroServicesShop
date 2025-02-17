using Shop.Services.EmailAPI.Message;
using Shop.Services.EmailAPI.Model.Dto;

namespace Shop.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailLog(string email);
        Task LogOrderPlaced(RewardMessage rewardMessage);

    }
}
