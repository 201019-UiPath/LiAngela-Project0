using System.Collections.Generic;

namespace StoreDB.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public List<Order> OrderHistory { get; set; }

        public List<ProductStock> ProductStocks { get; set; }
    }
}