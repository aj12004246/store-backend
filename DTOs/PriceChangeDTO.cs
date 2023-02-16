namespace store_be.DTOs
{
    public class PriceChangeDTO
    {
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public int ProductId { get; set; }
        public bool IsMap { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}
