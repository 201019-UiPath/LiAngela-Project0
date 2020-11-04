using System;
using System.Collections.Generic;
using Serilog;

using StoreDB;
using StoreDB.Models;
using StoreDB.Repos;
using StoreLib;

namespace StoreUI
{
    /// <summary>
    /// Shopping menu implementing IMenu interface persisting customer identity and store location
    /// </summary>
    public class ShopByLocationMenu : IMenu
    {
        private string userInput;

        private Dictionary<int, int> productMenuMapping;

        private Dictionary<int, int> cart;

        private decimal subtotal;

        private Location location;
        
        private Customer customer;

        private IOrderRepo orderRepo;

        private ILocationRepo locationRepo;

        private IProductRepo productRepo;

        private OrderService orderService;

        private LocationService locationService;

        private ProductService productService;

        public ShopByLocationMenu(Customer customer, Location location, IOrderRepo repo)
        {
            this.cart = new Dictionary<int, int>();
            this.customer = customer;
            this.location = location;
            this.orderRepo = repo;
            this.locationRepo = new LocationRepo(new StoreContext());
            this.productRepo = new ProductRepo(new StoreContext());
            this.orderService = new OrderService(repo);
            this.locationService = new LocationService(locationRepo);
            this.productService = new ProductService(productRepo);
        }

        public void Start()
        {
            Console.WriteLine($"\nWelcome to {location.Name}!");
            PrintInventory();
            do {
                Console.WriteLine($"\nWhat do you want to do at {location.Name}? (type \"x\" to go back)");
                Console.WriteLine("[0] View product details");
                Console.WriteLine("[1] Add/update a product in your order");
                Console.WriteLine("[2] View your cart");
                Console.WriteLine("[3] Place your order");
                Console.WriteLine($"[4] View all orders placed at {location.Name}");
                Console.WriteLine($"[5] View inventory at {location.Name}");
                userInput = Console.ReadLine();
                switch (userInput) {
                    case "0":
                        Console.WriteLine("\nChoose your product:");
                        PrintInventory();
                        userInput = Console.ReadLine();
                        int i = 0;
                        if (int.TryParse(userInput, out i)) {
                            if (productMenuMapping.ContainsKey(i)) {
                                Product product = productService.GetProductById(productMenuMapping.GetValueOrDefault(i));
                                Console.WriteLine($"\nProduct ID {product.ProductId}: {product.Name} ({product.Price.ToString("C")}): {product.Description}");
                            }
                        }
                        break;
                    case "1":
                        UpdateCart();
                        break;
                    case "2":
                        Console.WriteLine("\nYour cart:");
                        PrintCart();
                        break;
                    case "3":
                        Console.WriteLine("\nPlease confirm you want to place your order!\nYour cart:");
                        PrintCart();
                        Console.WriteLine($"[0] Place your order\n[1] Go back to {location.Name}");
                        userInput = Console.ReadLine();
                        if (userInput.Equals("0")) {
                            PlaceOrder();
                        } else if (!userInput.Equals("1")) {
                            userInput = "1";
                        }
                        break;
                    case "4":
                        PrintOrders();
                        break;
                    case "5":
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

        public void UpdateCart() {
            Console.WriteLine("\nChoose the product you want to add/update in your order:");
            PrintInventory();
            userInput = Console.ReadLine();
            int i = 0;
            if (int.TryParse(userInput, out i)) {
                int productId = productMenuMapping.GetValueOrDefault(i);
                int currentQuantity = cart.GetValueOrDefault(productId);
                Product product = productService.GetProductById(productId);
                Console.WriteLine($"\nYou currently have {currentQuantity} unit{(currentQuantity == 1 ? "" : "s")} of product {product.Name} in your cart!");
                Console.Write("Enter desired quantity: ");
                userInput = Console.ReadLine();
                if (int.TryParse(userInput, out i)) {
                    int quantity = i;
                    if (cart.ContainsKey(productId)) {
                        if (quantity < 1) {
                            cart.Remove(productId);
                        } else {
                            cart[productId] = quantity;
                        }
                    } else {
                        if (quantity > 0) {
                            cart.Add(productId, quantity);
                        }
                    }
                    Console.WriteLine("\nYour cart has been updated!\nUpdated cart:");
                    Log.Information("Cart has been updated");
                    PrintCart();
                }
            }
        }

        public void PrintCart() {
            List<string> itemList = productService.ViewCart(cart, out subtotal);
            foreach(string item in itemList) {
                Console.WriteLine(item);
            }
        }

        public void PrintOrders() {
            List<string> orderList = orderService.GetOrdersByLocationId(location.LocationId);
            if (orderList.Count == 0) {
                Console.WriteLine($"No orders have been placed at {location.Name} yet!");
            } else {
                foreach(string order in orderList) {
                    Console.WriteLine(order);
                }
                SortOrdersDifferently();
            }
        }

        public void PrintOrders(int sortOrderMethod) {
            List<string> orderList = orderService.GetOrdersByLocationId(location.LocationId, sortOrderMethod);
            foreach(string order in orderList) {
                Console.WriteLine(order);
            }
        }

        public void SortOrdersDifferently() {
            Console.WriteLine("\nSort orders differently?");
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

        public void PlaceOrder() {
            Order order = new Order();
            // explicitly assign OrderId also required in creating order's constituent OrderItems
            order.OrderId = orderService.GetNewOrderId();
            order.CustomerId = customer.CustomerId;
            order.LocationId = location.LocationId;
            order.ShippingAddress = customer.MailingAddress;
            order.OrderDate = DateTime.Now;
            order.TotalPrice = subtotal + Math.Round(decimal.Multiply(subtotal, Convert.ToDecimal(0.0825)), 2);
            orderService.PlaceOrder(order, cart);
            productService.UpdateProductStocks(location.LocationId, cart, false);
            Console.WriteLine("Your order has been placed!");
            Log.Information("Order has been placed");
            cart.Clear();
        }
    }
}