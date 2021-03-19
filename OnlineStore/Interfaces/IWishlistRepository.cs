using OnlineStore.DTOs;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<WishlistReturnDto>> GetProductsAsync(int userId);
        Task<Wishlist> GetWishlist(int userId, int productId);
        void AddProductOnWishlist(Wishlist wishlist);
        void DeleteProductFromWishlist(Wishlist wishlist);
    }
}
