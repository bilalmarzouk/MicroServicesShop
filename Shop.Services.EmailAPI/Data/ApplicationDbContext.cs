using Microsoft.EntityFrameworkCore;
using Shop.Services.EmailAPI.Model;


namespace Shop.Services.EmailAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<EmailLogger> EmailLoggers { get; set; }

    
    }
}
