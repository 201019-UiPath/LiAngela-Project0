using System;
using Serilog;

using StoreDB.Models;
using StoreDB.Repos;
using StoreLib;

namespace StoreUI
{
    /// <summary>
    /// Customer menu implementing IMenu interface
    /// </summary>
    public class CustomerMenu : IMenu
    {
        private string userInput;
        
        private ICustomerRepo repo;

        private ReturningCustomerMenu returningCustomerMenu;

        private CustomerService customerService;

        public CustomerMenu(ICustomerRepo repo)
        {
            this.repo = repo;
            this.customerService = new CustomerService(repo);
            this.returningCustomerMenu = new ReturningCustomerMenu(repo);
        }

        public void Start()
        {
            do {
                Console.Write("Are you a new customer [0] or a returning customer [1]? (type \"x\" to go back) ");
                userInput = Console.ReadLine();
                switch (userInput) {
                    case "0":
                        Customer newCustomer = CustomerSignup();
                        customerService.AddCustomer(newCustomer);
                        Console.WriteLine($"Customer {newCustomer.Name} added!");
                        Log.Information("Customer has been added");
                        break;
                    case "1":
                        returningCustomerMenu.Start();
                        break;
                    case "x":
                        Console.WriteLine("Going back...");
                        break;
                }
            } while (!userInput.Equals("x"));
        }

        public Customer CustomerSignup() {
            Customer customer = new Customer();
            Console.WriteLine("\nPlease sign up!");
            Console.Write("What is your name? ");
            customer.Name = Console.ReadLine();
            Console.Write("What is your phone number? ");
            customer.PhoneNumber = Console.ReadLine();
            Console.Write("What is your email address? ");
            customer.EmailAddress = Console.ReadLine();
            Console.Write("What is your mailing address? ");
            customer.MailingAddress = Console.ReadLine();
            return customer;
        }
    }
}