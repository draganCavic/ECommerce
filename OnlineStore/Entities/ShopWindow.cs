using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Entities
{
    public class ShopWindow
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
