using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shop.MessageBus;
using Shop.Services.ShoppingCart;
using Shop.Services.ShoppingCart.Data;
using Shop.Services.ShoppingCart.Extentions;
using Shop.Services.ShoppingCart.RabbitMQSender;
using Shop.Services.ShoppingCart.Service;
using Shop.Services.ShoppingCart.Service.IService;
using Shop.Services.ShoppingCart.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections"))
);
builder.Services.AddOpenApi();
var mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<APiAuthenticationHttpClientHandler>(); 
builder.Services.AddScoped<IRabiitMQCartMessageSender, RabiitMQCartMessageSender>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICouponService, CouponService>();
builder.AddAppAuthentication();
builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ApiSettings:ServiceUrls:ProductAPI"])).AddHttpMessageHandler<APiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(builder.Configuration["ApiSettings:ServiceUrls:CouponAPI"])).AddHttpMessageHandler<APiAuthenticationHttpClientHandler>(); 
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authrization string as following : `Bearer Generated-JWT Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id=JwtBearerDefaults.AuthenticationScheme,
            }
        },new string[]{ }
    }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}