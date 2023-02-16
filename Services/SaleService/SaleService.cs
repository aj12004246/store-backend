using store_be.DTOs;
using store_be.Models;

namespace store_be.Services.SaleService
{
    public class SaleService : ISaleService
    {
        private readonly DataContext _context;

        public SaleService(DataContext context)
        {

            _context = context;
        }
        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _context.Sales.ToListAsync();
        }
        //public async Task AddSaleAsync(Sale sale)
        //{
        //    _context.Sales.Add(sale);
        //    await _context.SaveChangesAsync();
        //}
        public async Task<Sale> AddSaleAsync(SaleDTO newSale)
        {
            var product = await _context.Products
                .Where(p => p.Id == newSale.ProductId)
                .Include(p => p.Sales)
                .FirstOrDefaultAsync();
            foreach (var csale in product.Sales)
            {
                if (csale == null)
                {
                    break;
                }
                // Checking that new sale is not within range of an existing sale 
                if (newSale.StartDate < csale.EndDate && newSale.EndDate > csale.StartDate)
                {
                    throw new Exception();
                }
            }


            var sale = new Sale
            {
                EndDate = newSale.EndDate,
                StartDate = newSale.StartDate,
                ProductId = newSale.ProductId,
                ProductName = product.ProductName,
                PercentageOff = newSale.PercentageOff,
                SalePrice = newSale.SalePrice
            };

            product.Sales.Add(sale);
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }
        public async Task RemoveSale(int id)
        {
            var foundSale = await _context.Sales.FindAsync(id);
            if (foundSale != null)
            {
                _context.Sales.Remove(foundSale);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Sale> UpdateSale(int id, SaleDTO sale)
        {
            var existingSale = await _context.Sales.FindAsync(id);
            if (existingSale != null)
            {
                existingSale.SalePrice = sale.SalePrice;
                existingSale.PercentageOff = sale.PercentageOff;
                existingSale.StartDate = sale.StartDate;
                existingSale.EndDate = sale.EndDate;
                existingSale.ProductId = sale.ProductId;
                existingSale.ProductName = sale.ProductName;


                await _context.SaveChangesAsync();
                return existingSale;
            }
            else
            {
                throw new Exception("Sale not found");
            }
        }
    }
}
