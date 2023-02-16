using System.Text.Json.Serialization;

namespace store_be.Models
{
    public class CartItem : CartItemBase
    {
        [JsonIgnore]
        public virtual Cart Cart { get; set; }
        public virtual int CartId { get; set; }
    }
}