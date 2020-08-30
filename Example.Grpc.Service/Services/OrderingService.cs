using System;
using System.Linq;
using System.Threading.Tasks;
using Example.Grpc.Service.Repositories;
using Grpc.Core;
using OrderModel = Example.Grpc.Service.Models;

namespace Example.Grpc.Service.Services
{
    /// <summary>
    /// Service class inheriting from Protocol Buffer compiler generated base
    /// </summary>
    public class OrderingService : Ordering.OrderingBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderingService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request,
            ServerCallContext context)
        {
            // Creating an Order from incoming request
            var order = new OrderModel.Order
            {
                CustomerName = request.Order.CustomerName,
                Value = request.Order.Value,
                Items = request.Order.Items.Select(x => new OrderModel.OrderItem {Name = x.Name}).ToList()
            };

            // Calling repository to persist the Order
            await _orderRepository.CreateOrderAsync(order);

            // Returning response with Id of created Order
            return new CreateOrderResponse
            {
                OrderId = order.Id.ToString()
            };
        }

        public override async Task<GetOrderByIdResponse> GetOrderById(GetOrderByIdRequest request,
            ServerCallContext context)
        {
            // Calling repository to fetch Order by its Id
            var orderModel = await _orderRepository.GetOrderByIdAsync(Guid.Parse(request.OrderId));

            // Mapping found Order model to Order returned in gRPC response
            var order = new Order
            {
                Id = orderModel.Id.ToString(),
                CustomerName = orderModel.CustomerName,
                Value = orderModel.Value
            };

            // Currently default Protocol Buffer compiler for .NET makes collections readonly,
            // that's why we can't create this collection during initialization
            order.Items.AddRange(orderModel.Items.Select(x => new OrderItem { Id = x.Id.ToString(), Name = x.Name }));

            // Returning response with found and mapped Order
            return new GetOrderByIdResponse
            {
                Order = order
            };
        }
    }
}