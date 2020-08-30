using System;
using System.Linq;
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

        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request,
            ServerCallContext context)
        {
            var order = new OrderModel.Order
            {
                CustomerName = request.Order.CustomerName,
                Value = request.Order.Value,
                Items = request.Order.Items.Select(x => new OrderModel.OrderItem {Name = x.Name}).ToList()
            };

            await _orderRepository.CreateOrderAsync(order);

            return new CreateOrderResponse
            {
                OrderId = order.Id.ToString()
            };
        }

        public override async Task<GetOrderByIdResponse> GetOrderById(GetOrderByIdRequest request,
            ServerCallContext context)
        {
            var orderModel = await _orderRepository.GetOrderByIdAsync(Guid.Parse(request.OrderId));

            var order = new Order
            {
                Id = orderModel.Id.ToString(),
                CustomerName = orderModel.CustomerName,
                Value = orderModel.Value
            };
            order.Items.AddRange(orderModel.Items.Select(x => new OrderItem {Id = x.Id.ToString(), Name = x.Name}));


            return new GetOrderByIdResponse
            {
                Order = order
            };
        }
    }
}