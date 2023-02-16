using Microsoft.AspNetCore.Cors.Infrastructure;
using store_be.Models;
using System.Xml.Schema;

namespace store_be.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext context;
        private readonly IProductService productService;

        public CartService(DataContext context, IProductService productService)
        {
            this.context = context;
            this.productService = productService;
        }

        public async Task<Cart> AddToCart(int productId, int accountId)
        {
            var myProduct = await context.Products.FindAsync(productId);
            var account = await context.Accounts
                    .Include(a => a.Carts)
                    .FirstOrDefaultAsync(a => a.Id == accountId);
            var existingCart = await context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.IsCheckedOut == false && c.AccountId == accountId);

            if (myProduct == null || account == null)
            {
                throw new Exception();
            }

            if (existingCart == null)
            {
                existingCart = new Cart
                {
                    Account = account,
                    AccountId = accountId,
                    Items = new List<CartItem>()
                };
                await context.Carts.AddAsync(existingCart);
            }

            var myCartItem = existingCart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (myCartItem == null)
            {
                CartItem cartItem = new()
                {
                    ProductId = productId,
                    Product = myProduct,
                    Quantity = 1,
                    Total = myProduct.Price,
                    Cart = existingCart,
                    CartId = existingCart.Id,
                    ProductName = myProduct.ProductName
                };
                existingCart.Total += myProduct.Price;
                await context.CartItems.AddAsync(cartItem);               
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

        public async Task<Cart> GetCurrentCart(int accountId)
        {
            var myCart = await context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.AccountId == accountId && c.IsCheckedOut == false);
            if (myCart == null)
            {
                throw new Exception("Cart not found");
            }
            var products = await productService.GetProducts();
            myCart.Total = 0;
            myCart.Items.ForEach(i =>
            {
                var product = products.FirstOrDefault(p => p.Id == i.ProductId);
                if (product != null && product.onSale == false) //newPrice: 2 oldPrice: 1 salePrice: 1
                {
                    i.Total = product.Price * i.Quantity;
                    myCart.Total += i.Total;
                }
                if(product != null && product.onSale == true)
                {
                    i.Total = product.salePrice * i.Quantity;
                    myCart.Total += i.Total;
                }
            });
            if (myCart.CouponCode == "") {
                await context.SaveChangesAsync();
                return myCart;
            }
            var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Code == myCart.CouponCode);
            if (coupon == null)
            {
                throw new Exception();
            }
            if(coupon != null)
            {

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
            }
            
            await context.SaveChangesAsync();
            return myCart;
        }

        public async Task DeleteCartItem(int productId, int accountId)
        {
            var myCart = await context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.AccountId == accountId && c.IsCheckedOut == false);

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
            context.CartItems.Remove(myItem);
            myCart.Items.Remove(myItem);
            await context.SaveChangesAsync();
        }

        public async Task<Cart> Checkout(int accountId)
        {
            var cart = await context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.IsCheckedOut == false && accountId == c.AccountId);
            if (cart == null)
            {
                throw new Exception();
            }
            if (cart.CouponCode != "")
            {
                var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Code.Equals(cart.CouponCode));
                if (coupon == null || coupon.UseLimit == 0 )
                {
                    throw new Exception("coupon incorrect");
                }
                if(DateTime.UtcNow >= coupon.EndDate || DateTime.UtcNow <= coupon.StartDate)
                {
                    throw new Exception("Coupon Expired");
                }
                if(coupon.UseLimit > 0)
                {
                    coupon.UseLimit-- ;
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

        public async Task<List<Cart>> GetPastOrders(int accountId)
        {
            var pastOrders = await context.Carts
                .Include(c => c.Items)
                .Where(c => c.AccountId == accountId && c.IsCheckedOut == true)
                .ToListAsync();
            if (pastOrders == null)
            {
                throw new Exception();
            }
            return pastOrders;
        }

        public async Task<Cart> ChangeItemQuantity(int accountId, int productId, int newQuantity)
        {
            var myCart = await context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.AccountId == accountId && c.IsCheckedOut == false);

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
                await DeleteCartItem(accountId, productId);
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
        public async Task<Coupon> AddCouponToCart(string CouponCode, int accountId)
        {
            var couponExists = await context.Coupons.FirstOrDefaultAsync(c => c.Code.Equals(CouponCode));
            if (couponExists == null || couponExists.UseLimit <= 0)
            {
                throw new Exception("Coupon Does not Exist or exceeded it's use limit");
            }
            if (DateTime.UtcNow >= couponExists.EndDate || DateTime.UtcNow <= couponExists.StartDate)
            {
                throw new Exception("Coupon Expired");
            }
            // Add check for guest cart
            var cart = await context.Carts
                .FirstOrDefaultAsync(c => c.AccountId == accountId && c.IsCheckedOut == false);
            if (cart == null)
            {
                throw new Exception("Cart Does not Exist");
            }
            cart.CouponCode = couponExists.Code;
            await context.SaveChangesAsync();
            return couponExists;

        }

        public async Task<Cart> RemoveCoupon(int accountId)
        {
            var myCart = await context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.AccountId == accountId && c.IsCheckedOut == false);
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