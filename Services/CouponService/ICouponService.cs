using store_be.Models;

namespace store_be.Services.CouponService
{
    public interface ICouponService
    {
        Task<List<Coupon>> GetAllCoupons();

        Task<Coupon> GetCouponById(int id);

        Task<Coupon> GetCouponByCode(string code);

        Task<Coupon> AddCoupon(Coupon coupon);

        Task<Coupon> UpdateCoupon(int id, Coupon coupon);

        Task DeleteCoupon(int id);



    }
}
