using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal AverageRating { get; set; } = 0;
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Photo> Photos { get; set; }

    }
}
