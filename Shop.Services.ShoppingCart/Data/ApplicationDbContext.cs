using Microsoft.EntityFrameworkCore;
using Shop.Services.ShoppingCart.Model;

namespace Shop.Services.ShoppingCart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }

       public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
    }
}
