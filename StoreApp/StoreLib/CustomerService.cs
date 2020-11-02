using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using StoreDB.Models;
using StoreDB.Repos;

namespace StoreLib
{
    public class CustomerService
    {
        private ICustomerRepo repo;

        public CustomerService(ICustomerRepo repo)
        {
            this.repo = repo;
        }

        public void AddCustomer(Customer newCustomer) {
            repo.AddCustomerAsync(newCustomer);
            Console.WriteLine($"Customer {newCustomer.Name} added!");
        }

        public List<string> GetCustomerList() {
            List<string> customerList = new List<string>();
            Task<List<Customer>> customerListTask = repo.GetAllCustomersAsync();
            foreach(Customer customer in customerListTask.Result) {
                customerList.Add($"Customer {customer.CustomerId}: {customer.Name}");
            }
            return customerList;
        }

        public Customer GetCustomerByEmailAddress(string customerEmailAddress)
        {
            return repo.GetCustomerByEmailAddress(customerEmailAddress);
        }
    }
}
