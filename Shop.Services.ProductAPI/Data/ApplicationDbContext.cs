using Microsoft.EntityFrameworkCore;
using Shop.Services.ProductAPI.Models;

namespace Shop.Services.ProductApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Magic Ball",
                Price = 10,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Games"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Magic Keyboard",
                Price = 33.99,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/602x402",
                CategoryName = "Office"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Magic chair",
                Price = 100.99,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/601x401",
                CategoryName = "Office"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Magic Table",
                Price = 105,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/600x400",
                CategoryName = "Kitchen"
            });
        }
    }
}
