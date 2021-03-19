using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Entities
{
    public class Wishlist
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
