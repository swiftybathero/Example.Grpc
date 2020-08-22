using System.Threading.Tasks;
using Example.Grpc.Service.Models;
using Example.Grpc.Service.Repositories;
using Grpc.Core;

namespace Example.Grpc.Service.Services
{
    public class OrderingService : Ordering.OrderingBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderingService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            var order = new Order
            {
                CustomerName = request.CustomerName,
                Value = request.Value
            };

            await _orderRepository.CreateOrderAsync(order);

            return new CreateOrderResponse
            {
                OrderId = order.Id.ToString()
            };
        }
    }
}