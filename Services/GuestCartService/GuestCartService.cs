using store_be.Models;
using System.Net;

namespace store_be.Services.GuestCartService
{
    public class GuestCartService: IGuestCartService
    {
        private readonly DataContext context;
        private readonly IProductService productService;
        private readonly string ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();

        public GuestCartService(DataContext context, IProductService productService)
        {
            this.context = context;
            this.productService = productService;
        }

        public async Task<GuestCart> GetCart()
        {
            var myCart = await context.GuestCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Ip == ip && c.IsCheckedOut == false);
            if (myCart == null)
            {
                throw new Exception("Cart not found");
            }
            myCart.Total = 0;
            var products = await productService.GetProducts();
            myCart.Items.ForEach(i =>
            {
                var product = products.FirstOrDefault(p => p.Id == i.ProductId);
                if (product != null && product.onSale == false)
                {
                    i.Total = product.Price * i.Quantity;
                    myCart.Total += i.Total;
                }
                if (product != null && product.onSale == true)
                {
                    i.Total = product.salePrice * i.Quantity;
                    myCart.Total += i.Total;
                }
            });
            if (myCart.CouponCode == "")
            {
                await context.SaveChangesAsync();
                return myCart;
            }
            var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Code == myCart.CouponCode);
            if (coupon == null)
            {
                throw new Exception();
            }
            if (coupon.AmountOff != 0)
            {
                if (coupon.AmountOff > myCart.Total)
                {
                    myCart.Total = 0;
                }
                else
                {
                    myCart.Total -= coupon.AmountOff;
                }
            }
            else
            {
                myCart.Total *= (1 - coupon.PercentageOff);
            }
            await context.SaveChangesAsync();
            return myCart;
        }
        public async Task<GuestCart> AddToCart(int productId)
        {
            var myProduct = await context.Products.FindAsync(productId);
            var existingCart = await context.GuestCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.IsCheckedOut == false && c.Ip == ip);

            if (myProduct == null)
            {
                throw new Exception();
            }

            if (existingCart == null)
            {
                existingCart = new GuestCart
                {
                    Items = new List<GuestCartItem>()
                };
                await context.GuestCarts.AddAsync(existingCart);
            }

            var myCartItem = existingCart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (myCartItem == null)
            {
                GuestCartItem cartItem = new()
                {
                    ProductId = productId,
                    Product = myProduct,
                    Quantity = 1,
                    Total = myProduct.Price,
                    GuestCart = existingCart,
                    GuestCartId = existingCart.Id,
                    ProductName = myProduct.ProductName
                };
                await context.GuestCartItems.AddAsync(cartItem);
                existingCart.Total += myProduct.Price;
            }
            else
            {
                myCartItem.Quantity++;
                myCartItem.Total += myProduct.Price;
                existingCart.Total += myProduct.Price;
            }
            await context.SaveChangesAsync();
            return existingCart;
        }

        public async Task<GuestCart> GuestCheckout()
        {
            var cart = await context.GuestCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.IsCheckedOut == false && c.Ip == ip);
            if (cart == null)
            {
                throw new Exception();
            }
            if (cart.CouponCode != "")
            {
                var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Code.Equals(cart.CouponCode));
                if (coupon == null || coupon.UseLimit == 0)
                {
                    cart.CouponCode = "";
                    await context.SaveChangesAsync();
                    throw new Exception("coupon incorrect");
                }
                if (coupon.StartDate.CompareTo(DateTime.UtcNow) >= 0 || coupon.EndDate.CompareTo(DateTime.UtcNow) <= 0)
                {
                    cart.CouponCode = "";
                    await context.SaveChangesAsync();
                    throw new Exception("Coupon Expired");
                }
                if (coupon.UseLimit > 0)
                {
                    coupon.UseLimit--;
                }
                await context.SaveChangesAsync();
            }
            cart.Items.ForEach(i =>
            {
                var newProduct = context.Products.Find(i.ProductId);
                if (newProduct == null) { throw new Exception(); }
                if (i.Quantity > newProduct.NumInStock) { throw new Exception("Not Enough " + i.Product.DisplayName + " In Stock"); };
                newProduct.NumInStock -= i.Quantity;
            });
            cart.IsCheckedOut = true;
            await context.SaveChangesAsync();
            return cart;
        }

        public async Task<GuestCart> ChangeItemQuantity(int productId, int newQuantity)
        {
            var myCart = await context.GuestCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Ip == ip && c.IsCheckedOut == false);

            if (myCart == null)
            {

                throw new Exception("Cart is null");

            }
            var myItem = myCart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (myItem == null)
            {

                throw new Exception("Product is null");

            }
            if (newQuantity <= 0)
            {
                await DeleteCartItem(productId);
                return myCart;
            }
            var PricePerProduct = myItem.Total / myItem.Quantity;
            var newItemTotal = PricePerProduct * newQuantity;
            myCart.Total += newItemTotal - myItem.Total;
            myItem.Total = newItemTotal;
            myItem.Quantity = newQuantity;
            await context.SaveChangesAsync();
            return myCart;
        }

        public async Task DeleteCartItem(int productId)
        {
            var myCart = await context.GuestCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Ip == ip && c.IsCheckedOut == false);

            if (myCart == null)
            {

                throw new Exception("Cart is null");

            }
            var myItem = myCart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (myItem == null)
            {

                throw new Exception("Product is null");

            }
            myCart.Total -= myItem.Total;
            context.GuestCartItems.Remove(myItem);
            myCart.Items.Remove(myItem);
            if(myCart.Items.Count == 0)
            {
                context.GuestCarts.Remove(myCart);
            }
            await context.SaveChangesAsync();
        }

        public async Task<Coupon> AddCouponToCart(string CouponCode)
        {
            var couponExists = await context.Coupons.FirstOrDefaultAsync(c => c.Code.Equals(CouponCode));
            if (couponExists == null || couponExists.UseLimit == 0)
            {
                throw new Exception("Coupon does not Exist or has exceeded the use limit");
            }
            if (couponExists.StartDate.CompareTo(DateTime.UtcNow) >= 0 || couponExists.EndDate.CompareTo(DateTime.UtcNow) <= 0)
            {
                throw new Exception("Coupon Expired");
            }
            // Add check for guest cart
            var cart = await context.GuestCarts
                .FirstOrDefaultAsync(c => c.Ip == ip && c.IsCheckedOut == false);
            if (cart == null)
            {
                throw new Exception("Cart Does not Exist");
            }
            cart.CouponCode = couponExists.Code;
            await context.SaveChangesAsync();
            return couponExists;
        }
        public async Task<GuestCart> RemoveCoupon()
        {
            var myCart = await context.GuestCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Ip == ip && c.IsCheckedOut == false);
            if (myCart == null)
            {
                throw new Exception();
            }
            myCart.CouponCode = "";
            await context.SaveChangesAsync();
            return myCart;
        }
    }
}

