using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using StoreDB.Models;
using StoreDB.Repos;

namespace StoreLib
{
    public class OrderService
    {
        private IOrderRepo repo;

        public OrderService(IOrderRepo repo)
        {
            this.repo = repo;
        }

        public int GetNewOrderId() {
            return repo.GetLastOrderId() + 1;
        }
        
        public void PlaceOrder(Order order, Dictionary<int, int> cart) {
            repo.AddOrder(order, cart);
        }

        public List<string> GetOrdersByCustomerId(int customerId)
        {
            List<string> orderList = new List<string>();
            Task<List<Order>> orderListTask = repo.GetOrdersByCustomerAsync(customerId);
            foreach(Order order in orderListTask.Result) {
                orderList.Add($"Order #{order.OrderId}: {order.OrderDate} {order.TotalPrice.ToString("C")} {order.Location.Name}");
            }
            return orderList;
        }

        public List<string> GetOrdersByCustomerId(int customerId, int sortOrderMethod)
        {
            List<string> orderList = new List<string>();
            Task<List<Order>> orderListTask = repo.GetOrdersByCustomerAsync(customerId, sortOrderMethod);
            foreach(Order order in orderListTask.Result) {
                orderList.Add($"Order #{order.OrderId}: {order.OrderDate} {order.TotalPrice.ToString("C")} {order.Location.Name}");
            }
            return orderList;
        }

        public List<string> GetOrdersByLocationId(int locationId)
        {
            List<string> orderList = new List<string>();
            Task<List<Order>> orderListTask = repo.GetOrdersByLocationAsync(locationId);
            foreach(Order order in orderListTask.Result) {
                orderList.Add($"Order #{order.OrderId}: {order.OrderDate} {order.TotalPrice.ToString("C")}");
            }
            return orderList;
        }
        
        public List<string> GetOrdersByLocationId(int locationId, int sortOrderMethod)
        {
            List<string> orderList = new List<string>();
            Task<List<Order>> orderListTask = repo.GetOrdersByLocationAsync(locationId, sortOrderMethod);
            foreach(Order order in orderListTask.Result) {
                orderList.Add($"Order #{order.OrderId}: {order.OrderDate} {order.TotalPrice.ToString("C")}");
            }
            return orderList;
        }
    }
}