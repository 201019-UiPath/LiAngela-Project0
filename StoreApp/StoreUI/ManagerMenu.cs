using System;
using System.Collections.Generic;

using StoreDB;
using StoreDB.Models;
using StoreDB.Repos;
using StoreLib;

namespace StoreUI
{
    public class ManagerMenu : IMenu
    {
        private string userInput;

        private ICustomerRepo customerRepo;

        private ILocationRepo locationRepo;

        private IProductRepo productRepo;

        private CustomerService customerService;

        private LocationService locationService;

        private ProductService productService;

        private StockingMenu stockingMenu;

        public ManagerMenu(ICustomerRepo repo)
        {
            this.customerRepo = repo;
            this.locationRepo = new LocationRepo(new StoreContext());
            this.productRepo = new ProductRepo(new StoreContext());
            this.customerService = new CustomerService(repo);
            this.locationService = new LocationService(locationRepo);
            this.productService = new ProductService(productRepo);
        }
        
        public void Start()
        {
            do {
                Console.WriteLine($"\nHi Manager! What do you want to do today? (type \"x\" to go back)");
                Console.WriteLine("[0] Stock your stores");
                Console.WriteLine("[1] View your customer list");
                userInput = Console.ReadLine();
                switch (userInput) {
                    case "0":
                        Console.WriteLine("\nChoose the store location you want to stock:");
                        List<string> locationList = locationService.GetLocationList();
                        foreach(string location in locationList) {
                            Console.WriteLine(location);
                        }
                        userInput = Console.ReadLine();
                        // validate user input
                        SelectLocation(Int32.Parse(userInput));
                        break;
                    case "1":
                        ViewCustomerList();
                        break;
                    case "x":
                        Console.WriteLine("Going back...");
                        break;
                }
            } while (!userInput.Equals("x"));
        }

        public void SelectLocation(int locationId) {
            Location location = locationService.GetLocationById(locationId);
            stockingMenu = new StockingMenu(location, productRepo);
            stockingMenu.Start();
        }

        public void ViewCustomerList() {
            List<string> customerList = customerService.GetCustomerList();
            foreach(string customer in customerList) {
                Console.WriteLine(customer);
            }
        }
    }
}