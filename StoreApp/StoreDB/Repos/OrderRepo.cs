using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using StoreDB.Models;

namespace StoreDB.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private StoreContext context;
        
        public OrderRepo(StoreContext context)
        {
            this.context = context;
        }

        public void AddOrder(Order order, Dictionary<int, int> cart)
        {
            foreach(KeyValuePair<int, int> item in cart) {
                OrderItem orderItem = new OrderItem();
                orderItem.OrderId = order.OrderId;
                orderItem.ProductId = item.Key;
                orderItem.QuantityOrdered = item.Value;
                context.OrderItems.AddAsync(orderItem);   
            }
            context.Orders.AddAsync(order);
            context.SaveChangesAsync();
        }

        public Task<List<OrderItem>> GetAllItemsInOrder(int orderId)
        {
            return context.OrderItems.Where(x => x.OrderId == orderId).ToListAsync();
        }

        public Task<List<Order>> GetAllOrdersAsync()
        {
            return context.Orders.Select(x => x).ToListAsync();
        }

        public int GetLastOrderId()
        {
            if (context.Orders.Select(x => x).Count() == 0) {
                return 0;
            } else {
                return context.Orders.Select(x => x.OrderId).Max();
            }
        }

        public Order GetOrderById(int orderId)
        {
            return context.Orders.Where(x => x.OrderId == orderId).SingleOrDefault();
        }

        public Task<List<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return context.Orders.Where(x => x.CustomerId == customerId)
            .Include("Location")
            .OrderBy(x => x.OrderDate)
            .ToListAsync();
        }

        public Task<List<Order>> GetOrdersByCustomerAsync(int customerId, int sortOrderMethod)
        {
            if (sortOrderMethod == 1) {
                return context.Orders.Where(x => x.CustomerId == customerId)
                .Include("Location")
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
            } else if (sortOrderMethod == 2) {
                return context.Orders.Where(x => x.CustomerId == customerId)
                .Include("Location")
                .OrderBy(x => x.TotalPrice)
                .ToListAsync();
            } else {
                return context.Orders.Where(x => x.CustomerId == customerId)
                .Include("Location")
                .OrderByDescending(x => x.TotalPrice)
                .ToListAsync();
            }
        }

        public Task<List<Order>> GetOrdersByLocationAsync(int locationId)
        {
            return context.Orders.Where(x => x.LocationId == locationId)
            .Include("Customer")
            .OrderBy(x => x.OrderDate)
            .ToListAsync();
        }

        public Task<List<Order>> GetOrdersByLocationAsync(int locationId, int sortOrderMethod)
        {
            if (sortOrderMethod == 1) {
                return context.Orders.Where(x => x.LocationId == locationId)
                .Include("Customer")
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
            } else if (sortOrderMethod == 2) {
                return context.Orders.Where(x => x.LocationId == locationId)
                .Include("Customer")
                .OrderBy(x => x.TotalPrice)
                .ToListAsync();
            } else {
                return context.Orders.Where(x => x.LocationId == locationId)
                .Include("Customer")
                .OrderByDescending(x => x.TotalPrice)
                .ToListAsync();
            }
        }
    }
}