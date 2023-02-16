using store_be.Models;

namespace store_be.Services.CouponService
{
    public class CouponService : ICouponService
    {
        private readonly DataContext context;

        public CouponService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<Coupon>> GetAllCoupons()
        {
            return await context.Coupons.ToListAsync();
        }

        public async Task<Coupon> GetCouponById(int id)
        {
            // maybe change this to be a separate function to replace repeated code in other functions
            var coupon = await context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                throw new FileNotFoundException("Coupon not found");
            }
            return coupon;
        }

        public async Task<Coupon> GetCouponByCode(string code)
        {
            var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Code.Equals(code));
            if (coupon == null)
            {
                throw new FileNotFoundException("Coupon code not found");
            }
            return coupon;
        }

        public async Task<Coupon> AddCoupon(Coupon coupon)
        {
            var couponCheck = await context.Coupons.FirstOrDefaultAsync(c => c.Code.Equals(coupon.Code));
            if (couponCheck == null)
            {
                context.Coupons.Add(coupon);
                await context.SaveChangesAsync();
                return coupon;
            }
            throw new Exception("Coupon code already exists");
        }

        public async Task<Coupon> UpdateCoupon(int id, Coupon updatedCoupon)
        {
            var existingCoupon = await context.Coupons.FindAsync(id);
            if (existingCoupon == null)
            {
                throw new FileNotFoundException("Could not update - Coupon does not exist");
            }
            existingCoupon.Code = updatedCoupon.Code;
            existingCoupon.StartDate = updatedCoupon.StartDate;
            existingCoupon.EndDate = updatedCoupon.EndDate;
            existingCoupon.UseLimit = updatedCoupon.UseLimit;
            existingCoupon.AmountOff = updatedCoupon.AmountOff;
            existingCoupon.PercentageOff = updatedCoupon.PercentageOff;

            await context.SaveChangesAsync();
            return existingCoupon;
        }

        public async Task DeleteCoupon(int id)
        {
            var couponToDelete = await context.Coupons.FindAsync(id);
            if (couponToDelete == null)
            {
                throw new FileNotFoundException("Could not delete - Coupon not found");
            }
            context.Coupons.Remove(couponToDelete);
            await context.SaveChangesAsync();
        }

    } // End of class
}
