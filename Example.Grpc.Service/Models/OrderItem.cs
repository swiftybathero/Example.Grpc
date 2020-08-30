using System;

namespace Example.Grpc.Service.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
