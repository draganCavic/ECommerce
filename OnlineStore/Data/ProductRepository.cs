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
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateProduct(ProductCreateDto productDto, int userId)
        {
            var product = _mapper.Map<Product>(productDto);
            product.UserId = userId;
            _context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<ProductDto>> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .Where(x => x.Name == name)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return await _context.Products
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetUserProductsAsync(int userId)
        {
            return await _context.Products.Where(x => x.UserId == userId)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
