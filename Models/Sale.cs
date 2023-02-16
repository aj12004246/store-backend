using System.Text.Json.Serialization;

namespace store_be.Models
{
    public class Sale
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Id { get; set; }
        public double SalePrice { get; set; }
        public double PercentageOff { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}
