using Microsoft.EntityFrameworkCore;
using store_be.Models;

namespace store_be
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<PriceChange> PriceChanges { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<GuestCart> GuestCarts { get; set; }
        public DbSet<GuestCartItem> GuestCartItems { get; set; }
    }
}
