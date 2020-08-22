using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Grpc.Service.Models;

namespace Example.Grpc.Service.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private static readonly IList<Order> Orders = new List<Order>();

        public Task CreateOrderAsync(Order order)
        {
            order.Id = Guid.NewGuid();

            Orders.Add(order);

            return Task.CompletedTask;
        }

        public Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return Task.FromResult(Orders.FirstOrDefault(x => x.Id == orderId));
        }
    }
}
