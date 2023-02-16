using store_be.Models;
using System.Text.Json.Serialization;

namespace store_be.DTOs
{
    public class SaleDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float SalePrice { get; set; }
        public float PercentageOff { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}
