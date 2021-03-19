using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        IWishlistRepository WishlistRepository { get; }
        IReviewRepository ReviewRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
