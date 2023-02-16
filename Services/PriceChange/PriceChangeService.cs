using store_be.DTOs;
using store_be.Models;

namespace store_be.Services
{
    public class PriceChangeService : IPriceChangeService
    {
        private readonly DataContext context;

        public PriceChangeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<PriceChange> Schedule(PriceChangeDTO request)
        {

            var product = await context.Products
                .Where(p => p.Id == request.ProductId)
                .Include(p => p.PriceChanges)
                .FirstOrDefaultAsync();
            if (product == null) 
            { 
                throw new Exception(); 
            }
            var priceChange = new PriceChange
            {
                Price = request.Price,
                Product = product,
                ProductId = request.ProductId,
                StartDate = request.StartDate,
                ProductName = product.ProductName,
                IsMap= request.IsMap,
            };
            product.PriceChanges.Add(priceChange);
            context.PriceChanges.Add(priceChange);
            await context.SaveChangesAsync();
            return priceChange;
        }
        public async Task<List<PriceChange>> GetPriceChangesById(int productId)
        {
            var priceChanges = await context.PriceChanges.Where(c => c.ProductId == productId).ToListAsync();
            if (priceChanges == null) 
            {
                throw new Exception();    
            }
            return priceChanges;
        }

        public async Task<List<PriceChange>> GetPriceChanges()
        {
            return await context.PriceChanges.ToListAsync();
        }

        public async Task<PriceChange> UpdatePriceChange(int id, PriceChangeDTO updateRequest)
        {
            var existingPriceChange = await context.PriceChanges.FindAsync(id);
            if (existingPriceChange != null)
            {
                existingPriceChange.StartDate = updateRequest.StartDate;
                existingPriceChange.Price = updateRequest.Price;
                existingPriceChange.IsMap = updateRequest.IsMap;
                existingPriceChange.ProductId = updateRequest.ProductId;
                existingPriceChange.ProductName = updateRequest.ProductName;

                await context.SaveChangesAsync();
                return existingPriceChange;
            }

            throw new Exception();
        }

        public async Task DeletePriceChange(int id)
        {
            var toBeDeleted = context.PriceChanges.Find(id);
            if (toBeDeleted != null)
            {
                context.PriceChanges.Remove(toBeDeleted);
                await context.SaveChangesAsync();
                return;
            }
            throw new Exception();
        }
    }
}
