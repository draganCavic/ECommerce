using Microsoft.EntityFrameworkCore;
using OnlineStore.DTOs;
using OnlineStore.Entities;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly DataContext _context;

        public WishlistRepository(DataContext context)
        {
            _context = context;
        }

        public void AddProductOnWishlist(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
        }

        public void DeleteProductFromWishlist(Wishlist wishlist)
        {
            _context.Wishlists.Remove(wishlist);
        }

        public async Task<IEnumerable<WishlistReturnDto>> GetProductsAsync(int userId)
        {
            return await (from user in _context.Users
                          orderby user.Id
                          select new WishlistReturnDto
                          {
                              Id = user.Id,
                              UserName = user.UserName,
                              Products = (from wishlist in user.Wishlist
                                         join product in _context.Products
                                         on wishlist.ProductId
                                         equals product.Id
                                         orderby product.DateAdded descending
                                         select new ProductDto
                                         {
                                             Name = product.Name,
                                             Description = product.Description,
                                             AverageRating = product.AverageRating,
                                             DateAdded = product.DateAdded,
                                             StockQuantity = product.StockQuantity,
                                             Price = product.Price,
                                             Photos = product.Photos
                                         })
                          }).Where(u => u.Id == userId).ToListAsync();
        }

        public async Task<Wishlist> GetWishlist(int userId, int productId)
        {
            return await _context.Wishlists.FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);
        }
    }
}
