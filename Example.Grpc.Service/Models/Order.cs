using System;
using System.Collections.Generic;

namespace Example.Grpc.Service.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public double Value { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
    }
}
