namespace StoreDB.Models
{
    public class ProductStock
    {
        public int Id { get; set; }

        public int LocationId { get; set; }
        
        public int ProductId { get; set; }

        public Location Location { get; set; }
        
        public Product Product { get; set; }

        public int QuantityStocked { get; set; }
    }
}