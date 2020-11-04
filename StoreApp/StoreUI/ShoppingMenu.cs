using System;
using System.Collections.Generic;

using StoreDB;
using StoreDB.Models;
using StoreDB.Repos;
using StoreLib;

namespace StoreUI
{
    /// <summary>
    /// Shopping menu implementing IMenu interface persisting customer identity
    /// </summary>
    public class ShoppingMenu : IMenu
    {
        private string userInput;

        private Customer customer;

        private Dictionary<int, int> locationMenuMapping;

        private IOrderRepo orderRepo;

        private ILocationRepo locationRepo;

        private IProductRepo productRepo;

        private OrderService orderService;

        private LocationService locationService;

        private ProductService productService;

        private ShopByLocationMenu shopByLocationMenu;
        
        public ShoppingMenu(Customer customer, IOrderRepo repo)
        {
            this.customer = customer;
            this.orderRepo = repo;
            this.locationRepo = new LocationRepo(new StoreContext());
            this.productRepo = new ProductRepo(new StoreContext());
            this.orderService = new OrderService(repo);
            this.locationService = new LocationService(locationRepo);
            this.productService = new ProductService(productRepo);
        }

        public void Start()
        {
            do {
                Console.WriteLine("\nWe are so happy you want to make your home energy-efficient! Do you want to...");
                Console.WriteLine("[0] browse available products by location or\n[1] look for a certain product? (type \"x\" to go back)");
                userInput = Console.ReadLine();
                switch (userInput) {
                    case "0":
                        Console.WriteLine("\nSelect a location:");
                        List<string> locationList = locationService.GetLocationList();
                        foreach(string location in locationList) {
                            Console.WriteLine(location);
                        }
                        userInput = Console.ReadLine();
                        int i = 0;
                        if (int.TryParse(userInput, out i)) {
                            SelectLocation(i);
                        }
                        break;
                    case "1":
                        Console.WriteLine("\nChoose your product:");
                        List<string> productList = productService.GetProductList();
                        foreach(string product in productList) {
                            Console.WriteLine(product);
                        }
                        ChooseProduct();
                        break;
                    case "x":
                        Console.WriteLine("Going back...");
                        break;
                }
            } while (!userInput.Equals("x"));
        }

        public void SelectLocation(int locationId) {
            Location location = locationService.GetLocationById(locationId);
            shopByLocationMenu = new ShopByLocationMenu(customer, location, orderRepo);
            shopByLocationMenu.Start();
        }

        public void ChooseProduct() {
            userInput = Console.ReadLine();
            int i = 0;
            if (int.TryParse(userInput, out i)) {
                int productId = i;
                Product product = productService.GetProductById(productId);
                List<string> locationStockList = productService.ViewProductStockByProductId(productId, out locationMenuMapping);
                if (locationStockList.Count == 0) {
                    Console.WriteLine($"{product.Name} is currently out of stock!");
                } else {
                    Console.WriteLine($"\n{product.Name} is in stock at the following locations:");
                    foreach(string location in locationStockList) {
                        Console.WriteLine(location);
                    }
                    Console.WriteLine("\nProceed to shop at one of these locations? (type \"x\" to go back)");
                    userInput = Console.ReadLine();
                    if (userInput.Equals("x")) {
                        Start();
                    }
                    if (int.TryParse(userInput, out i)) {
                        if (locationMenuMapping.ContainsKey(i)) {
                            int locationId = locationMenuMapping.GetValueOrDefault(i);
                            SelectLocation(locationId);
                        }
                    }
                }
            }
        }
    }
}