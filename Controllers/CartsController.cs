using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using store_be.Models;
using store_be.Services.CartService;
using store_be.Services.GuestCartService;

namespace store_be.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {

        private readonly ICartService cartService;
        private readonly IGuestCartService guestCartService;

        public CartsController(ICartService cartService, IGuestCartService guestCartService)
        {
            this.cartService = cartService;
            this.guestCartService = guestCartService;
        }

        [HttpGet("{accountId}")]
        public async Task<ActionResult<CartBase>> Get(int accountId)
        {
            try
            {
                if (accountId == 0)
                {
                    var guestCart = await guestCartService.GetCart();
                    return Ok(guestCart);
                }
                var myCart = await cartService.GetCurrentCart(accountId);
                return Ok(myCart);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("addItem/{accountId}/{productId}")]
        public async Task<ActionResult<CartBase>> AddToCart(int productId, int accountId)
        {
            try
            {
                if (accountId == 0)
                {
                    var guestCart = await guestCartService.AddToCart(productId);
                    return Ok(guestCart);
                }
                var cart = await cartService.AddToCart(productId, accountId);
                return Ok(cart);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }


        [HttpDelete("removeItem/{accountId}/{productId}")]
        public async Task<ActionResult> DeleteFromCart(int productId, int accountId)
        {
            try
            {
                if (accountId == 0)
                {
                    await guestCartService.DeleteCartItem(productId);
                    return Ok();
                }
                await cartService.DeleteCartItem(productId, accountId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPut("Checkout/{accountId}")]
        public async Task<ActionResult<CartBase>> Checkout(int accountId)
        {
            try
            {
                if(accountId == 0)
                {
                    var guestCart = await guestCartService.GuestCheckout();
                    return Ok(guestCart);
                }
                var existingCart = await cartService.Checkout(accountId);
                return Ok(existingCart);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpGet("OrderHistory/{accountId}")]
        public async Task<ActionResult<List<CartBase>>> GetPastOrders(int accountId)
        {
            try
            {
                var pastOrders = await cartService.GetPastOrders(accountId);
                return Ok(pastOrders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ChangeQuantity/{accountId}/{productId}/{newQuantity}")]
        public async Task<ActionResult<CartBase>> ChangeItemQuantity(int accountId, int productId, int newQuantity)
        {
            try
            {
                if (accountId == 0)
                {
                    var guestCart = await guestCartService.ChangeItemQuantity(productId, newQuantity);
                    return Ok(guestCart);
                }
                var newCart = await cartService.ChangeItemQuantity(accountId, productId, newQuantity);
                return Ok(newCart);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("AddCoupon/{couponCode}/{accountId}")]
        public async Task<ActionResult<Coupon>> AddCoupon(string couponCode,int accountId)
        {
            try {
                if(accountId == 0)
                {
                    var guestCartCoupon = await guestCartService.AddCouponToCart(couponCode);
                    return Ok(guestCartCoupon);
                }
                var cartCoupon = await cartService.AddCouponToCart(couponCode, accountId);
                return Ok(cartCoupon);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("RemoveCoupon/{accountId}")]
        public async Task<ActionResult<CartBase>> RemoveCoupon(int accountId)
        {
            try
            {
                if (accountId == 0)
                {
                    var guestCartCoupon = await guestCartService.RemoveCoupon();
                    return Ok(guestCartCoupon);
                }
                var cartCoupon = await cartService.RemoveCoupon(accountId);
                return Ok(cartCoupon);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

}