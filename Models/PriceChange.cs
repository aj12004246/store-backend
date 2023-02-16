using System.Text.Json.Serialization;

namespace store_be.Models
{
    public class PriceChange
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public bool IsMap { get; set; }

        public bool IsApplied { get; set; }

    }
}
