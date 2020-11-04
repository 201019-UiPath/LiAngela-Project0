using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Serilog;

using StoreDB;
using StoreDB.Models;
using StoreDB.Repos;
using StoreLib;

namespace StoreUI
{
    /// <summary>
    /// Returning customer menu implementing IMenu interface persisting customer identity
    /// </summary>
    public class ReturningCustomerMenu : IMenu
    {
        private string userInput;

        private Customer customer;

        private ICustomerRepo customerRepo;

        private IOrderRepo orderRepo;

        private CustomerService customerService;

        private OrderService orderService;

        private ShoppingMenu shoppingMenu;

        public ReturningCustomerMenu(ICustomerRepo repo)
        {
            this.customerRepo = repo;
            this.orderRepo = new OrderRepo(new StoreContext());
            this.customerService = new CustomerService(repo);
            this.orderService = new OrderService(orderRepo);
        }

        public void Start()
        {
            EstablishCustomerIdentity();
            Log.Information("Customer has established identity");
            do {
                Console.WriteLine($"\nHi {customer.Name}! What do you want to do today? (type \"x\" to go back)");
                Console.WriteLine("[0] Go shopping to make your home more energy-efficient!");
                Console.WriteLine("[1] View your orders");
                Console.WriteLine("[2] View your personal information");
                userInput = Console.ReadLine();
                switch (userInput) {
                    case "0":
                        shoppingMenu.Start();
                        break;
                    case "1":
                        PrintOrders();
                        break;
                    case "2":
                        ViewCustomerInformation();
                        break;
                    case "x":
                        Console.WriteLine("Going back...");
                        break;
                }
            } while (!userInput.Equals("x"));
        }

        public void EstablishCustomerIdentity() {
            Console.WriteLine("\nWelcome back! Please enter your email address to establish your identity:");
            userInput = Console.ReadLine();
            // add input validation with regex
            customer = customerService.GetCustomerByEmailAddress(userInput);
            shoppingMenu = new ShoppingMenu(customer, orderRepo);
        }

        public void PrintOrders() {
            List<string> orderList = orderService.GetOrdersByCustomerId(customer.CustomerId);
            if (orderList.Count == 0) {
                Console.WriteLine("You have not yet placed an order!");
            } else {
                foreach(string order in orderList) {
                    Console.WriteLine(order);
                }
                SortOrdersDifferently();
            }
        }

        public void PrintOrders(int sortOrderMethod) {
            List<string> orderList = orderService.GetOrdersByCustomerId(customer.CustomerId, sortOrderMethod);
            foreach(string order in orderList) {
                Console.WriteLine(order);
            }
        }

        public void SortOrdersDifferently() {
            Console.WriteLine("\nSort your orders differently?");
            Console.WriteLine("[0] No");
            Console.WriteLine("[1] Order by date descending");
            Console.WriteLine("[2] Order by total price ascending");
            Console.WriteLine("[3] Order by total price descending");
            userInput = Console.ReadLine();
            switch (userInput) {
                case "0":
                    break;
                case "1":
                    PrintOrders(1);
                    break;
                case "2":
                    PrintOrders(2);
                    break;
                case "3":
                    PrintOrders(3);
                    break;
                case "x":
                    userInput = "0";
                    break;
                default:
                    break;
            }
        }
        
        public void ViewCustomerInformation() {
            Console.WriteLine("\nYour information on file:");
            Console.WriteLine($"Name: {customer.Name}");
            Console.WriteLine($"Phone number: {customer.PhoneNumber}");
            Console.WriteLine($"Email address: {customer.EmailAddress}");
            Console.WriteLine($"Mailing address: {customer.MailingAddress}");
        }
    }
}