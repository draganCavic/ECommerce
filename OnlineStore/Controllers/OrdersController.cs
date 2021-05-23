using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTOs;
using OnlineStore.Entities;
using OnlineStore.Extensions;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(Order orderDto)
        {
            if (User.GetUserId() != orderDto.UserId) return Unauthorized();

            _unitOfWork.OrderRepository.CreateOrder(orderDto);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to create order");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, Order orderDto)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderById(id);
            if (order == null) return NotFound();
            if (order.Status != orderDto.Status)
            {
                order.Status = orderDto.Status;
            }

            _unitOfWork.OrderRepository.UpdateOrder(order);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update order");
        }

        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderById(orderId);

            if (User.GetUserId() != order.UserId) return Unauthorized();

            _unitOfWork.OrderRepository.RemoveOrder(order);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to delete order");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetOrders();

            return Ok(orders);
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderById(id);

            return Ok(order);
        }
        
        [HttpGet("buyer")]
        public async Task<ActionResult<OrderDto>> GetOrdersByBuyerId(int id)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByBuyerId(id);

            return Ok(orders);
        }
    }
}
