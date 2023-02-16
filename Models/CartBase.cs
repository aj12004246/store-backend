namespace store_be.Models
{
    public class CartBase
    {
        public int Id { get; set; }
        public bool IsCheckedOut { get; set; } = false;
        public double Total { get; set; }
        public string CouponCode { get; set; } = string.Empty;
    }
}