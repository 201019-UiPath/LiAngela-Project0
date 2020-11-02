using System.Collections.Generic;
using System.Threading.Tasks;

using StoreDB.Models;
using StoreDB.Repos;

namespace StoreLib
{
    public class ProductService
    {
        private IProductRepo repo;

        public ProductService(IProductRepo repo)
        {
            this.repo = repo;
        }

        public List<string> GetProductList() {
            List<string> productList = new List<string>();
            Task<List<Product>> productListTask = repo.GetAllProductsAsync();
            foreach(Product product in productListTask.Result) {
                productList.Add($"[{product.ProductId}] {product.Name} ({product.Price.ToString("C")})");
            }
            return productList;
        }

        public Product GetProductById(int productId)
        {
            return repo.GetProductById(productId);
        }

        public List<string> ViewCart(Dictionary<int, int> cart, out decimal subtotal) {
            List<string> cartList = new List<string>();
            subtotal = 0;
            foreach(KeyValuePair<int, int> item in cart) {
                Product cartItem = repo.GetProductById(item.Key);
                cartList.Add($"  {item.Value} x ({cartItem.Name} @ {cartItem.Price.ToString("C")}/unit) = {(cartItem.Price * item.Value).ToString("C")}");
                subtotal += cartItem.Price * item.Value;
            }
            cartList.Add("--------------------");
            cartList.Add($"Subtotal: {subtotal.ToString("C")}");
            return cartList;
        }

        public List<string> ViewProductsToStock(Dictionary<int, int> productsToStock) {
            decimal subtotal = 0;
            List<string> productList = new List<string>();
            foreach(KeyValuePair<int, int> item in productsToStock) {
                Product product = repo.GetProductById(item.Key);
                productList.Add($"  {item.Value} x ({product.Name} @ {product.Price.ToString("C")}/unit) = {(product.Price * item.Value).ToString("C")}");
                subtotal += product.Price * item.Value;
            }
            productList.Add("--------------------");
            productList.Add($"Subtotal: {subtotal.ToString("C")}");
            return productList;
        }

        public List<string> ViewProductStockByLocation(int locationId, out Dictionary<int, int> menuMapping) {
            List<string> productStockList = new List<string>();
            menuMapping = new Dictionary<int, int>();
            Task<List<ProductStock>> productStockListTask = repo.GetProductStockByLocation(locationId);
            foreach(ProductStock productStock in productStockListTask.Result) {
                productStockList.Add($"[{menuMapping.Count}] {productStock.Product.Name} ({productStock.Product.Price.ToString("C")}): {productStock.QuantityStocked} in stock");
                menuMapping.Add(menuMapping.Count, productStock.Product.ProductId);
            }
            return productStockList;
        }

        public List<string> ViewProductStockByProductId(int productId, out Dictionary<int, int> menuMapping) {
            List<string> productStockList = new List<string>();
            menuMapping = new Dictionary<int, int>();
            Task<List<ProductStock>> productStockListTask = repo.GetProductStockByProductId(productId);
            foreach(ProductStock productStock in productStockListTask.Result) {
                productStockList.Add($"[{menuMapping.Count}] {productStock.QuantityStocked} in stock at {productStock.Location.Name} ({productStock.Location.Address})");
                menuMapping.Add(menuMapping.Count, productStock.Location.LocationId);
            }
            return productStockList;
        }

        public void UpdateProductStocks(int locationId, Dictionary<int, int> cart, bool isManager) {
            foreach(KeyValuePair<int, int> item in cart) {
                repo.UpdateProductStock(locationId, item.Key, isManager ? item.Value : item.Value * -1);
            }
        }
    }
}