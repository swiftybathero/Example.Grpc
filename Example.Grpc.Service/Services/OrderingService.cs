using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace Example.Grpc.Service.Services
{
    public class OrderingService : Ordering.OrderingBase
    {
        public override Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CreateOrderResponse {OrderId = Guid.NewGuid().ToString()});
        }
    }
}