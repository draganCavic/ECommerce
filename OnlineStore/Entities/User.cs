using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }

        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }
        public ICollection<ShopWindow> ShopWindows { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Wishlist> Wishlist { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
