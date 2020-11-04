using System;
using System.Collections.Generic;
using Serilog;

using StoreDB.Models;
using StoreDB.Repos;
using StoreLib;

namespace StoreUI
{
    /// <summary>
    /// Stocking menu implementing IMenu interface persisting store location
    /// </summary>
    public class StockingMenu : IMenu
    {
        private string userInput;

        private Dictionary<int, int> productMenuMapping;

        private Dictionary<int, int> productsToStock;
        
        private Location location;

        private IProductRepo productRepo;

        private ProductService productService;

        public StockingMenu(Location location, IProductRepo repo)
        {
            this.productsToStock = new Dictionary<int, int>();
            this.location = location;
            this.productRepo = repo;
            this.productService = new ProductService(repo);
        }

        public void Start()
        {
            Console.WriteLine($"\nWelcome to {location.Name}!");
            PrintInventory();
            do {
                Console.WriteLine($"\nWhat do you want to do at {location.Name}? (type \"x\" to go back)");
                Console.WriteLine("[0] Add a product/quantity to be stocked");
                Console.WriteLine("[1] View your products/quantities to be stocked");
                Console.WriteLine("[2] Stock the store");
                Console.WriteLine($"[3] View inventory at {location.Name}");
                userInput = Console.ReadLine();
                switch (userInput) {
                    case "0":
                        StockProduct();
                        break;
                    case "1":
                        PrintProductsToStock();
                        break;
                    case "2":
                        Console.WriteLine("\nPlease confirm you want to stock the store with the following products/quantities!");
                        PrintProductsToStock();
                        Console.WriteLine($"[0] Stock the store\n[1] Go back to {location.Name}");
                        userInput = Console.ReadLine();
                        if (userInput.Equals("0")) {
                            StockStore();
                        } else if (!userInput.Equals("1")) {
                            userInput = "1";
                        }
                        break;
                    case "3":
                        PrintInventory();
                        break;
                    case "x":
                        Console.WriteLine("Going back...");
                        break;
                }
            } while (!userInput.Equals("x"));
        }

        public void PrintInventory() {
            Console.WriteLine($"\n{location.Name} product inventory:");
            List<string> inventoryList = productService.ViewProductStockByLocation(location.LocationId, out productMenuMapping);
            foreach(string product in inventoryList) {
                Console.WriteLine(product);
            }
        }

        public void StockProduct() {
            Console.WriteLine("\nChoose the product you want to stock:");
            List<string> productList = productService.GetProductList();
            foreach(string item in productList) {
                Console.WriteLine(item);
            }
            userInput = Console.ReadLine();
            int i = 0;
            if (int.TryParse(userInput, out i)) {
                int productId = i;
                Product product = productService.GetProductById(productId);
                Console.Write($"Enter quantity of {product.Name} to add: ");
                userInput = Console.ReadLine();
                if (int.TryParse(userInput, out i)) {
                    int quantity = i;
                    if (productsToStock.ContainsKey(productId)) {
                        if (quantity == 0) {
                            productsToStock.Remove(productId);
                        } else {
                            productsToStock[productId] = quantity;
                        }
                    } else {
                        if (quantity > 0) {
                            productsToStock.Add(productId, quantity);
                        }
                    }
                    Console.WriteLine("\nYou have updated your products/quantities to be stocked!");
                    Log.Information("Products/quantities to be stocked have been updated");
                    PrintProductsToStock();    
                }
            }
        }

        public void PrintProductsToStock() {
            List<string> itemList = productService.ViewProductsToStock(productsToStock);
            foreach(string item in itemList) {
                Console.WriteLine(item);
            }
        }

        public void StockStore() {
            productService.UpdateProductStocks(location.LocationId, productsToStock, true);
            Console.WriteLine("\nYou have stocked the store!");
            Log.Information("Store has been stocked");
            PrintInventory();
            productsToStock.Clear();
        }
    }
}