using store_be.DTOs;
using store_be.Models;

namespace store_be.Services.SaleService
{
    public interface ISaleService
    {
        Task<Sale> AddSaleAsync(SaleDTO newSale);
        Task<List<Sale>> GetAllSalesAsync();
        Task RemoveSale(int id);

        Task<Sale> UpdateSale(int id, SaleDTO sale);
    }
}
