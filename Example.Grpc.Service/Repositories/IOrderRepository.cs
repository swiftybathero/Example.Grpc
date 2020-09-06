using System;
using System.Threading.Tasks;
using Example.Grpc.Service.Models;

namespace Example.Grpc.Service.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(Guid orderId);
    }
}
