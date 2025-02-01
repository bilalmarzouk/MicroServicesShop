using Microsoft.AspNetCore.Authentication.Cookies;
using Shop.Web.Service;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options => {
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.LoginPath = "/Auth/login";
        options.AccessDeniedPath = "/Auth/AcessDenied";
    }
    );
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();
Details.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
Details.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
Details.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
Details.ShoppingCartApi = builder.Configuration["ServiceUrls:ShoppingCartApi"];

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
