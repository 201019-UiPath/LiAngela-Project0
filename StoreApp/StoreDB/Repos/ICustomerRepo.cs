using System.Threading.Tasks;
using System.Collections.Generic;

using StoreDB.Models;

namespace StoreDB.Repos
{
    public interface ICustomerRepo
    {
         Task<List<Customer>> GetAllCustomersAsync();

         void AddCustomerAsync(Customer customer);

         Customer GetCustomerByEmailAddress(string customerEmailAddress);
    }
}