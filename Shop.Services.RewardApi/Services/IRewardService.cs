
using Shop.Services.RewardApi.Message;

namespace Shop.Services.RewardApi.Services
{
    public interface IRewardService
    {
        Task UpdateReward(RewardMessage rewardsMEssage);
       

    }
}
