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
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public void CreateOrder(Order order)
        {
            _context.Orders.Add(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByBuyerId(int buyerId)
        {
            var ordersForReturn = await (from order in _context.Orders
                                  orderby order.Id
                                  select new OrderDto
                                  {
                                      Id = order.Id,
                                      Amount = order.Amount,
                                      PurchaseDate = order.PurchaseDate,
                                      City = order.City,
                                      Country = order.Country,
                                      Address = order.Address,
                                      Postcode = order.Postcode,
                                      Status = order.Status,
                                      UserId = order.UserId,
                                      Items = (from item in order.Items
                                               join prod in _context.Products on item.ProductId equals prod.Id
                                               orderby item.OrderId
                                               select new ItemDto
                                               {
                                                   OrderId = item.OrderId,
                                                   Price = item.Price,
                                                   Quantity = item.Quantity,
                                                   UserId = prod.UserId
                                               })
                                  }).Where(u => u.UserId == buyerId).ToListAsync();
            return ordersForReturn;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders()
        {
            var ordersForReturn = await(from order in _context.Orders
                                        orderby order.Id
                                        select new OrderDto
                                        {
                                            Id = order.Id,
                                            Amount = order.Amount,
                                            PurchaseDate = order.PurchaseDate,
                                            City = order.City,
                                            Country = order.Country,
                                            Address = order.Address,
                                            Postcode = order.Postcode,
                                            Status = order.Status,
                                            UserId = order.UserId,
                                            Items = (from item in order.Items
                                                     join prod in _context.Products on item.ProductId equals prod.Id
                                                     orderby item.OrderId
                                                     select new ItemDto
                                                     {
                                                         OrderId = item.OrderId,
                                                         Price = item.Price,
                                                         Quantity = item.Quantity,
                                                         UserId = prod.UserId
                                                     })
                                        }).ToListAsync();
            return ordersForReturn;
        }

        public void RemoveOrder(Order order)
        {
            _context.Orders.Remove(order);
        }

        public void UpdateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersBySellerId(int sellerId)
        {
            var ordersForReturn = await(from order in _context.Orders
                                        orderby order.Id
                                        select new OrderDto
                                        {
                                            Id = order.Id,
                                            Amount = order.Amount,
                                            PurchaseDate = order.PurchaseDate,
                                            City = order.City,
                                            Country = order.Country,
                                            Address = order.Address,
                                            Postcode = order.Postcode,
                                            Status = order.Status,
                                            UserId = order.UserId,
                                            Items = (from item in order.Items
                                                     join prod in _context.Products on item.ProductId equals prod.Id
                                                     orderby item.OrderId
                                                     select new ItemDto
                                                     {
                                                         OrderId = item.OrderId,
                                                         Price = item.Price,
                                                         Quantity = item.Quantity,
                                                         UserId = prod.UserId
                                                     }).Where(x => x.UserId == sellerId)
                                        }).ToListAsync();
            ordersForReturn.Where(o => (o.Items.ToArray()).Length > 1);
            return ordersForReturn;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
    }
}
