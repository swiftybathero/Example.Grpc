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
            var createOrderRequest = new CreateOrderRequest
            {
                Order = new Order
                {
                    Value = 15.5,
                    CustomerName = "Sample Customer Name",
                    Items =
                    {
                        new OrderItem
                        {
                            Name = "First Item"
                        },
                        new OrderItem
                        {
                            Name = "Second Item"
                        }
                    }
                }
            };

            var serviceUrl = Configuration.GetValue<string>("OrderingServiceUrl");

            using var channel = GrpcChannel.ForAddress(serviceUrl);
            var client = new Ordering.OrderingClient(channel);

            Info("Creating an Order ...");
            var createOrderResponse = await client.CreateOrderAsync(createOrderRequest);

            Info($"Order created | OrderId: {createOrderResponse.OrderId}");
            var orderRequest = new GetOrderByIdRequest
            {
                OrderId = createOrderResponse.OrderId
            };

            Info($"Getting Order | OrderId: {orderRequest.OrderId}");
            var orderResponse = await client.GetOrderByIdAsync(orderRequest);

            Info($"Order found | {orderResponse}");
        }

        private static void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}
