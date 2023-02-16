using store_be.Models;

namespace store_be.Services.CartService
{
    public interface ICartService
    {
        Task<Cart> GetCurrentCart(int accountId);
        Task<Cart> AddToCart(int productId, int accountId);
        Task DeleteCartItem(int accountId, int productId);
        Task<Cart> Checkout(int accountId);
        Task<List<Cart>> GetPastOrders(int accountId);
        Task<Cart> ChangeItemQuantity(int accountId, int productId, int newQuantity);
        Task<Coupon> AddCouponToCart(string CouponCode,int accountId);
        Task<Cart> RemoveCoupon(int accountId);
    }
}
