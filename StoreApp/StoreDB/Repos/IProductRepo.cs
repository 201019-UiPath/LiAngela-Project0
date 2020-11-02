using System.Threading.Tasks;
using System.Collections.Generic;

using StoreDB.Models;

namespace StoreDB.Repos
{
    public interface IProductRepo
    {
         Task<List<Product>> GetAllProductsAsync();

         int GetLastProductStockId();

         Product GetProductById(int productId);

         Task<List<ProductStock>> GetProductStockByLocation(int locationId);

         Task<List<ProductStock>> GetProductStockByProductId(int productId);

         ProductStock GetProductStockByLocationProductId(int locationId, int productId);
        
         void UpdateProductStock(int locationId, int productId, int quantityChange);
    }
}