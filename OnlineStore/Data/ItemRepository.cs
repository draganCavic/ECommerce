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
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ItemRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);
        }
        public async Task<IEnumerable<ItemDto>> GetItemsByOrder(int orderId)
        {
            return await _context.Items
                .Where(x => x.OrderId == orderId)
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public void CreateItem(ItemDto itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);

            _context.Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            _context.Items.Remove(item);
        }

        public void UpdateItem(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
