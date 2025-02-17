using Microsoft.EntityFrameworkCore;
using Shop.Services.RewardApi.Data;
using Shop.Services.RewardApi.Message;
using Shop.Services.RewardApi.Models;
using System.Text;

namespace Shop.Services.RewardApi.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;
        public RewardService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

  

        public async Task UpdateReward(RewardMessage rewardsMessage)
        {
            
            try
            {
                Reward reward = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardActivity = rewardsMessage.RewardActivity,
                    UserId = rewardsMessage.UserId,
                    RewardDate = DateTime.Now
                };
                await using var _db = new ApplicationDbContext(_dbOptions);
                await _db.Rewards.AddAsync(reward); ;
                await _db.SaveChangesAsync();
              
            }
            catch (Exception ex)
            {
             
            }
        }
    }
}
