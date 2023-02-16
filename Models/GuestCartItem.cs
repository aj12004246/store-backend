using System.Text.Json.Serialization;

namespace store_be.Models
{
    public class GuestCartItem: CartItemBase
    {
        [JsonIgnore]
        public GuestCart GuestCart { get; set; }
        public int GuestCartId { get; set;}
    }
}
