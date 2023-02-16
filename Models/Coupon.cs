namespace store_be.Models
{
    public class Coupon
    {
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UseLimit { get; set; }
        public double AmountOff { get; set; }
        public double PercentageOff { get; set; }
        public int Id { get; set; }
    }
}
