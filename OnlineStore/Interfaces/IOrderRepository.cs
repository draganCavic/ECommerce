using OnlineStore.DTOs;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        void RemoveOrder(Order order);
        void UpdateOrder(Order order);
        Task<IEnumerable<OrderDto>> GetOrdersByBuyerId(int buyerId);
        Task<IEnumerable<OrderDto>> GetOrdersBySellerId(int sellerId);
        Task<IEnumerable<OrderDto>> GetOrders();
        Task<Order> GetOrderById(int id);
    }
}
