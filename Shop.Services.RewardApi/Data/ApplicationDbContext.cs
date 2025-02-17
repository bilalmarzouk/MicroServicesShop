using Microsoft.EntityFrameworkCore;
using Shop.Services.RewardApi.Models;



namespace Shop.Services.RewardApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Reward> Rewards { get; set; }

    
    }
}
