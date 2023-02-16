using System.Net;
using System.Text.Json.Serialization;

namespace store_be.Models
{
    public class GuestCart : CartBase
    {
        public string Ip { get; set; } = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
        public List<GuestCartItem> Items { get; set; }
    }
}
