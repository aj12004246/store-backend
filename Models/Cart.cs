using System.Text.Json.Serialization;

namespace store_be.Models
{
    public class Cart : CartBase
    {
        [JsonIgnore]
        public Account Account { get; set; }
        public int AccountId { get; set; }
        public List<CartItem> Items { get; set; }
    }
}