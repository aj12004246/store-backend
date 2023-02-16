namespace store_be.Models
{
    public class Account
    {
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int Id { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
