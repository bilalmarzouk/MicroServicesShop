using Microsoft.EntityFrameworkCore;
using Shop.Services.CouponAPI.Models;

namespace Shop.Services.CouponAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                DiscountAmount = 10,
                CopounCode = "10BM",
                MinAmount = 20

            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                DiscountAmount = 20,
                CopounCode = "20BM",
                MinAmount = 40

            });
        }
    }
}
