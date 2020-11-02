using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using StoreDB.Models;

namespace StoreDB.Repos
{
    public class CustomerRepo : ICustomerRepo
    {
        private StoreContext context;
        
        public CustomerRepo(StoreContext context)
        {
            this.context = context;
        }

        public void AddCustomerAsync(Customer customer)
        {
            context.Customers.AddAsync(customer);
            context.SaveChangesAsync();
        }

        public Task<List<Customer>> GetAllCustomersAsync()
        {
            return context.Customers.Select(x => x).ToListAsync();
        }

        public Customer GetCustomerByEmailAddress(string customerEmailAddress)
        {
            return context.Customers.Where(x => x.EmailAddress == customerEmailAddress).SingleOrDefault();
        }
    }
}