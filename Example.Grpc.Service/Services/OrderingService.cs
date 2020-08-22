using System;
using System.Threading.Tasks;
using Example.Grpc.Service.Repositories;
using Grpc.Core;
using OrderModel = Example.Grpc.Service.Models;

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
            var order = new OrderModel.Order
            {
                CustomerName = request.Order.CustomerName,
                Value = request.Order.Value
            };

            await _orderRepository.CreateOrderAsync(order);

            return new CreateOrderResponse
            {
                OrderId = order.Id.ToString()
            };
        }

        public override async Task<GetOrderByIdResponse> GetOrderById(GetOrderByIdRequest request, ServerCallContext context)
        {
            var order = await _orderRepository.GetOrderByIdAsync(Guid.Parse(request.OrderId));

            return new GetOrderByIdResponse
            {
                Order = new Order
                {
                    Id = order.Id.ToString(),
                    CustomerName = order.CustomerName,
                    Value = order.Value
                }
            };
        }
    }
}