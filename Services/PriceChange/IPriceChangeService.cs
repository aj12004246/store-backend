using Microsoft.AspNetCore.Mvc;
using store_be.DTOs;
using store_be.Models;

namespace store_be.Services
{
    public interface IPriceChangeService
    {
        Task<List<PriceChange>> GetPriceChangesById(int productId);
        Task<PriceChange> Schedule(PriceChangeDTO request);
        Task<List<PriceChange>> GetPriceChanges();
        Task<PriceChange> UpdatePriceChange(int id, PriceChangeDTO updateRequest);
        Task DeletePriceChange(int id);
    }
}
