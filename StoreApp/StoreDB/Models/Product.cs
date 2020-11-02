using System.Collections.Generic;

namespace StoreDB.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        
        public decimal Price { get; set; }

        // public string Description { get; set; }

        public List<ProductStock> ProductStocks { get; set; }

        public List<OrderItem> ProductOrders { get; set; }
    }
}