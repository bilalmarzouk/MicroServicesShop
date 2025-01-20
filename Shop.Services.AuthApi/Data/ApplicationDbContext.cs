using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Services.AuthApi.Model;


namespace Shop.Services.AuthApi.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUsers>
    {
        public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
        }
    }
}
