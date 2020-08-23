using System;
using System.Threading.Tasks;
using Example.Grpc.Service.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Example.Grpc.Client
{
    static class Program
    {
        private static IConfiguration _configuration;
        public static IConfiguration Configuration => _configuration ??= InitConfiguration();

        private static IConfiguration InitConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        static async Task Main(string[] args)
        {
            Info("Creating an Order ...");

            var newOrder = new NewOrder
            {
                Value = 15.5,
                CustomerName = "Sample Customer Name"
            };

            var serviceUrl = Configuration.GetSection("OrderingServiceUrl").Value;

            Info($"Calling gRPC service to create an Order: {serviceUrl}");

            using var channel = GrpcChannel.ForAddress(serviceUrl);

            var client = new Ordering.OrderingClient(channel);
            var createOrderRequest = new CreateOrderRequest
            {
                Order = newOrder
            };
            var createOrderResponse = await client.CreateOrderAsync(createOrderRequest);

            Info($"Created Order with Id: {createOrderResponse.OrderId}");

            Info($"Fetching Order with Id: {createOrderResponse.OrderId}");

            var orderRequest = new GetOrderByIdRequest
            {
                OrderId = createOrderResponse.OrderId
            };

            var orderResponse = await client.GetOrderByIdAsync(orderRequest);
            var order = orderResponse.Order;

            Console.WriteLine($"Fetched Order: OrderId: {order.Id} | CustomerName: {order.CustomerName} | Value: {order.Value}");
        }

        private static void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}
