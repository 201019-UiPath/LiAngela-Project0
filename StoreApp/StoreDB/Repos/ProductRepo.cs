using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using StoreDB.Models;

namespace StoreDB.Repos
{
    public class ProductRepo : IProductRepo
    {
        private StoreContext context;

        public ProductRepo(StoreContext context)
        {
            this.context = context;
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            return context.Products.Select(x => x).ToListAsync();
        }

        public int GetLastProductStockId()
        {
            if (context.ProductStocks.Select(x => x).Count() == 0) {
                return 0;
            } else {
                return context.ProductStocks.Select(x => x.Id).Max();
            }
        }

        public Product GetProductById(int productId)
        {
            return context.Products.Where(x => x.ProductId == productId).SingleOrDefault();
        }
        
        public Task<List<ProductStock>> GetProductStockByLocation(int locationId)
        {
            return context.ProductStocks.Where(x => x.LocationId == locationId)
            .Include("Product")
            .ToListAsync();
        }

        public Task<List<ProductStock>> GetProductStockByProductId(int productId)
        {
            return context.ProductStocks.Where(x => x.ProductId == productId)
            .Include("Location")
            .ToListAsync();
        }

        public ProductStock GetProductStockByLocationProductId(int locationId, int productId)
        {
            return context.ProductStocks.Where(x => x.ProductId == productId && x.LocationId == locationId).SingleOrDefault();
        }

        public void UpdateProductStock(int locationId, int productId, int quantityChange)
        {
            ProductStock productStock = GetProductStockByLocationProductId(locationId, productId);
            if (productStock == null) {
                ProductStock newProductStock = new ProductStock();
                newProductStock.Id = GetLastProductStockId() + 1;
                newProductStock.LocationId = locationId;
                newProductStock.ProductId = productId;
                newProductStock.QuantityStocked = quantityChange;
                context.ProductStocks.Add(newProductStock);
            } else if (productStock.QuantityStocked + quantityChange == 0) {
                context.ProductStocks.Remove(productStock);
            } else {
                productStock.QuantityStocked += quantityChange;
                context.ProductStocks.Update(productStock);
            }
            context.SaveChanges();
        }
    }
}