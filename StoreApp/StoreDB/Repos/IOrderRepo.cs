using System.Threading.Tasks;
using System.Collections.Generic;

using StoreDB.Models;

namespace StoreDB.Repos
{
    public interface IOrderRepo
    {
         void AddOrder(Order order, Dictionary<int, int> cart);

         Task<List<OrderItem>> GetAllItemsInOrder(int orderId);
         
         Task<List<Order>> GetAllOrdersAsync();

         int GetLastOrderId();

         Order GetOrderById(int orderId);

         Task<List<Order>> GetOrdersByCustomerAsync(int customerId);

         Task<List<Order>> GetOrdersByCustomerAsync(int customerId, int sortOrderMethod);

         Task<List<Order>> GetOrdersByLocationAsync(int locationId);

         Task<List<Order>> GetOrdersByLocationAsync(int locationId, int sortOrderMethod);    
    }
}