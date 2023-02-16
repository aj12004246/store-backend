using System.Text.Json.Serialization;

namespace store_be.Models
{
    public class CartItemBase
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}