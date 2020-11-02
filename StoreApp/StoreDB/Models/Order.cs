using System;
using System.Collections.Generic;

namespace StoreDB.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int LocationId { get; set; }

        public Customer Customer { get; set; }

        public Location Location { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}