using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Status { get; set; } = "Processing";

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
