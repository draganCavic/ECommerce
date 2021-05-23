using OnlineStore.DTOs;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    interface IItemRepository
    {
        void CreateItem(ItemDto order);
        void RemoveItem(Item order);
        void UpdateItem(Item order);
        Task<IEnumerable<ItemDto>> GetItemsByOrder(int orderId);
        Task<Item> GetItemByIdAsync(int id);
    }
}
