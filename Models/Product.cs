using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace store_be.Models
{
    public class Product
    {
        public string ProductName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Img { get; set; } = string.Empty;
        public virtual IEnumerable<Category> Categories { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public bool Discontinued { get; set; }
        public double Price { get; set; }
        public virtual ICollection<PriceChange> PriceChanges { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Map { get; set; }
        public int QuantityAtCost { get; set; }
        public DateTime AvailableOn { get; set; }
        public int NumInStock { get; set; }

        public bool onSale { get; set; }

        public double salePrice { get; set; }

    }
}
