using System;
using System.Threading.Tasks;
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
            Console.WriteLine("Hello World!");
        }
    }
}
