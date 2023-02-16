global using Microsoft.EntityFrameworkCore;
using store_be.Services;
using store_be.Services.AccountService;
using store_be.Services.CartService;
using store_be.Services.CategoryService;
using store_be.Services.CouponService;
using store_be.Services.SaleService;
using store_be.Services.GuestCartService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "localhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
        });
});

builder.Services.AddDbContext<store_be.DataContext>(options =>
{
    options.UseSqlServer("Server=localhost;Initial Catalog=StoreDB;Integrated Security=False;User Id=sa;Password=Your_password123;MultipleActiveResultSets=True");
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IPriceChangeService, PriceChangeService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IGuestCartService, GuestCartService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("localhost");

app.Run();
