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
            // Creating Request with Order data, that we want to create
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

            // Fetching gRPC Service URL from appsettings.json
            var serviceUrl = Configuration.GetValue<string>("OrderingServiceUrl");

            // Creating channel and client
            using var channel = GrpcChannel.ForAddress(serviceUrl);
            var client = new Ordering.OrderingClient(channel);

            Info("Creating an Order ...");
            // Invoking the client to create the Order and getting response from the Service
            var createOrderResponse = await client.CreateOrderAsync(createOrderRequest);

            Info($"Order created | OrderId: {createOrderResponse.OrderId}");
            // Creating Request passing Id of created Order to fetch its data
            var orderRequest = new GetOrderByIdRequest
            {
                OrderId = createOrderResponse.OrderId
            };

            Info($"Getting Order | OrderId: {orderRequest.OrderId}");
            // Invoking the client to fetch created Order (with its Ids generated server-side)
            var orderResponse = await client.GetOrderByIdAsync(orderRequest);

            // Printing Order data
            Info($"Order found | {orderResponse.Order}");
        }

        private static void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}
