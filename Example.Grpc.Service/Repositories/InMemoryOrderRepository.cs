﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Grpc.Service.Models;

namespace Example.Grpc.Service.Repositories
{
    /// <summary>
    /// Simple In-Memory implementation for demonstration purposes
    /// </summary>
    public class InMemoryOrderRepository : IOrderRepository
    {
        private static readonly IDictionary<Guid, Order> Orders = new Dictionary<Guid, Order>();

        public Task CreateOrderAsync(Order order)
        {
            order.Id = Guid.NewGuid();
            foreach (var item in order.Items)
            {
                item.Id = Guid.NewGuid();
            }

            Orders.Add(order.Id, order);

            return Task.CompletedTask;
        }

        public Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return Task.FromResult(Orders.FirstOrDefault(x => x.Key == orderId).Value);
        }
    }
}
