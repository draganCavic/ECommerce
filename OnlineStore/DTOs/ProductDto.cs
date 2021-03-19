using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal AverageRating { get; set; } = 0;
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
