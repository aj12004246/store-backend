using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using store_be.Models;
using store_be.Services.CouponService;

namespace store_be.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService couponService;

        // need error/exception handling
        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Coupon>>> GetCoupons()
        {
            return Ok(await couponService.GetAllCoupons());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Coupon>> GetCouponById(int id)
        {
            return Ok(await couponService.GetCouponById(id));
        }

        [HttpGet("byCode/{code}")]
        public async Task<ActionResult<Coupon>> GetCouponByCode(string code)
        {
            return Ok(await couponService.GetCouponByCode(code));
        }

        [HttpPost]
        public async Task<ActionResult<Coupon>> CreateCoupon(Coupon coupon)
        {
            return Ok(await couponService.AddCoupon(coupon));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Coupon>> UpdateCoupon(int id, Coupon coupon)
        {
            return Ok(await couponService.UpdateCoupon(id, coupon));
        }

        [HttpDelete("{id}")]
        public async Task DeleteCoupon(int id)
        {
            await couponService.DeleteCoupon(id);
            return;
        }

    }// End of class
}
