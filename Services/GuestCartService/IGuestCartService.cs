using Microsoft.AspNetCore.Mvc;
using store_be.Models;

namespace store_be.Services.GuestCartService
{
    public interface IGuestCartService
    {
        Task<GuestCart> GetCart();
        Task<GuestCart> AddToCart(int productId);
        Task<GuestCart> GuestCheckout();
        Task<GuestCart> ChangeItemQuantity(int productId, int newQuantity);
        Task DeleteCartItem(int productId);
        Task<Coupon> AddCouponToCart(string couponCode);
        Task<GuestCart> RemoveCoupon();
    }
}
