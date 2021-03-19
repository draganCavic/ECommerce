using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetProductReviews(int productId)
        {
            return await _context.Reviews
                .Where(x => x.ProductId == productId)
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
        }

        public void RemoveReview(Review review)
        {
            _context.Reviews.Remove(review);
        }

        public async Task<Review> GetProductReview(int productId, int userId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);
        }
    }
}
