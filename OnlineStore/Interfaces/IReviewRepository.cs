using OnlineStore.DTOs;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IReviewRepository
    {
        void AddReview(Review review);
        void RemoveReview(Review review);
        Task<IEnumerable<ReviewDto>> GetProductReviews(int productId);
        Task<Review> GetProductReview(int productId, int userId);
    }
}
