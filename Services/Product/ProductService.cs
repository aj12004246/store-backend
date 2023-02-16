using Microsoft.AspNetCore.Mvc;
using store_be.Models;

namespace store_be.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext context;

        public ProductService(DataContext context)
        {
            this.context = context;
        }
        public async Task<List<Product>> GetProducts()
        {
            var products = await context.Products
                .Include(p => p.PriceChanges)
                .Include(p => p.Categories)
                .Include(p => p.Sales)
                .ToListAsync();
            products
                .ForEach(p =>
                {
                    foreach (var change in p.PriceChanges)
                    {
                        if (!change.IsApplied && change.StartDate.CompareTo(DateTime.UtcNow) <= 0)
                        {
                            if (change.IsMap)
                            {
                                p.Map = change.Price;
                            }
                            else
                            {
                                p.Price = change.Price;
                            }
                            change.IsApplied = true;
                        }
                    }
                    var isActive = false;
                    foreach (var sale in p.Sales) 
                    { 
                        if (sale.StartDate.CompareTo(DateTime.UtcNow) <=0 && sale.EndDate.CompareTo(DateTime.UtcNow) >=0)
                        {
                            isActive = true;
                            if(sale.SalePrice == 0)
                            {
                                p.salePrice =  p.Price * (1 - sale.PercentageOff);
                                break;
                            }
                            p.salePrice = sale.SalePrice;
                            break;
                        }
                    }
                    p.onSale = isActive;
                });
            await context.SaveChangesAsync();
            return products;
        }

        public async Task<Product> AddProduct(Product product)
        {
            var productNameExists = await context.Products.FirstOrDefaultAsync(p => p.ProductName == product.ProductName);
            if (productNameExists == null)
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                return product;
            }
            throw new Exception("Product name already exists");
        }

        public async Task<Product> UpdateProduct(int id, Product productRequest)
        {
            var existingProduct = await context.Products.FindAsync(id);
            if (existingProduct != null)
            {
                existingProduct.ProductName = productRequest.ProductName;
                existingProduct.AvailableOn = productRequest.AvailableOn;
                existingProduct.Price = productRequest.Price;
                existingProduct.PriceChanges = productRequest.PriceChanges;
                existingProduct.Categories = productRequest.Categories;
                existingProduct.DisplayName = productRequest.DisplayName;
                existingProduct.Img = productRequest.Img;
                existingProduct.Sales = productRequest.Sales;
                existingProduct.Discontinued = productRequest.Discontinued;
                existingProduct.Description = productRequest.Description;
                existingProduct.Map = productRequest.Map;
                existingProduct.QuantityAtCost = productRequest.QuantityAtCost;
                existingProduct.NumInStock = productRequest.NumInStock;
                existingProduct.onSale = productRequest.onSale;
                existingProduct.salePrice= productRequest.salePrice;

                await context.SaveChangesAsync();
                return existingProduct;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public async Task DeleteProduct(int id)
        {
            var productToDelete = await context.Products.FindAsync(id);
            if (productToDelete == null)
            {
                throw new Exception("Product not found");
            }
            context.Products.Remove(productToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null )
            {
                throw new Exception("Product not found");
            }
            return product;
        }
    }
}
