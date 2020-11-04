using System;
using Xunit;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using StoreDB;
using StoreDB.Models;
using StoreDB.Repos;
using StoreLib;

namespace StoreTest
{
    public class StoreTest
    {
        private ICustomerRepo customerRepo;

        private IOrderRepo orderRepo;

        private ILocationRepo locationRepo;

        private IProductRepo productRepo;
        
        private void Seed(StoreContext testContext) {
            testContext.Customers.AddRange(testCustomers);
            testContext.Locations.AddRange(testLocations);
            testContext.Products.AddRange(testProducts);
            testContext.ProductStocks.AddRange(testProductStocks);
            testContext.SaveChanges();
        }

        private readonly Customer testCustomer = new Customer()
        {
            Name = "Test Customer",
            PhoneNumber = "(000) 000 0000",
            EmailAddress = "test@customer.com",
            MailingAddress = "000 St"
        };

        private readonly Order testOrder = new Order()
        {
            OrderId = 1,
            CustomerId = 1,
            LocationId = 1,
            OrderDate = DateTime.Now
        };

        private Dictionary<int, int> testCart = new Dictionary<int, int>();

        private readonly List<Customer> testCustomers = new List<Customer>()
        {
            new Customer {
                Name = "Test1 Customer",
                PhoneNumber = "(000) 000 0000",
                EmailAddress = "test1@customer.com",
                MailingAddress = "100 St"
            },
            new Customer {
                Name = "Testing Testing",
                PhoneNumber = "(000) 000 0000",
                EmailAddress = "test2@customer.com",
                MailingAddress = "200 St"
            }
        };

        private readonly List<Location> testLocations = new List<Location>()
        {
            new Location {LocationId = 1, Name = "Location 1", PhoneNumber = "(111) 111 1111", Address = "111 Main St"},
            new Location {LocationId = 2, Name = "Location 2", PhoneNumber = "(211) 111 1111", Address = "211 Main St"}
        };

        private readonly List<Product> testProducts = new List<Product>()
        {
            new Product {ProductId = 1, Name = "Solar Panel", Price = 100, Description = "It is a solar panel"},
            new Product {ProductId = 2, Name = "Insulated Window", Price = 50, Description = "Window has insulation"},
            new Product {ProductId = 3, Name = "Energy-efficient Refrigerator", Price = 200, Description = "Energy-efficient refrigerator runs on 10% less electricity than normal refrigerator runs on"},
            new Product {ProductId = 4, Name = "Energy-efficient Dishwasher", Price = 201, Description = "Energy-efficient dishwasher runs on 10% less electricity than normal dishwasher runs on"},
            new Product {ProductId = 5, Name = "Energy-efficient Microwave", Price = 202, Description = "Energy-efficient microwave runs on 10% less electricity than normal microwave runs on"},
            new Product {ProductId = 6, Name = "Energy-efficient Oven", Price = 203, Description = "Energy-efficient oven runs on 10% less electricity than normal oven runs on"},
            new Product {ProductId = 7, Name = "High-efficiency Washer", Price = 204, Description = "High-efficiency front-load washer"},
            new Product {ProductId = 8, Name = "High-efficiency Dryer", Price = 205, Description = "High-efficiency front-load dryer"}
        };
        
        private readonly List<ProductStock> testProductStocks = new List<ProductStock>()
        {
            new ProductStock {Id = 1, LocationId = 1, ProductId = 1, QuantityStocked = 10},
            new ProductStock {Id = 2, LocationId = 1, ProductId = 2, QuantityStocked = 10},
            new ProductStock {Id = 3, LocationId = 2, ProductId = 3, QuantityStocked = 10},
            new ProductStock {Id = 4, LocationId = 2, ProductId = 4, QuantityStocked = 10},
            new ProductStock {Id = 5, LocationId = 2, ProductId = 5, QuantityStocked = 10},
            new ProductStock {Id = 6, LocationId = 3, ProductId = 6, QuantityStocked = 10},
            new ProductStock {Id = 7, LocationId = 3, ProductId = 7, QuantityStocked = 10},
            new ProductStock {Id = 8, LocationId = 3, ProductId = 8, QuantityStocked = 10}
        };

        [Fact]
        public void AddCustomerShouldAddCustomer()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("AddCustomerShouldAddCustomer").Options;
            using var testContext = new StoreContext(options);
            customerRepo = new CustomerRepo(testContext);

            customerRepo.AddCustomerAsync(testCustomer);

            using var assertContext = new StoreContext(options);
            Assert.NotNull(assertContext.Customers.Single(c => c.Name == testCustomer.Name));
        }

        [Fact]
        public async void GetAllCustomersShouldGetAllCustomers()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("GetAllCustomersShouldGetAllCustomers").Options;
            using var testContext = new StoreContext(options);
            customerRepo = new CustomerRepo(testContext);
            Seed(testContext);

            List<Customer> customerList = await customerRepo.GetAllCustomersAsync();

            using var assertContext = new StoreContext(options);
            Assert.NotNull(customerList);
            Assert.Equal(2, customerList.Count);
        }

        [Fact]
        public void GetCustomerByEmailAddressShouldGetGustomerByEmailAddress()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("GetCustomerByEmailAddressShouldGetGustomerByEmailAddress").Options;
            using var testContext = new StoreContext(options);
            customerRepo = new CustomerRepo(testContext);
            Seed(testContext);

            Customer customer = customerRepo.GetCustomerByEmailAddress("test1@customer.com");

            using var assertContext = new StoreContext(options);
            Assert.NotNull(customer);
            Assert.Equal("Test1 Customer", customer.Name);
        }

        [Fact]
        public async void GetAllLocationsShouldGetAllLocations()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("GetAllLocationsShouldGetAllLocations").Options;
            using var testContext = new StoreContext(options);
            locationRepo = new LocationRepo(testContext);
            Seed(testContext);

            List<Location> locationList = await locationRepo.GetAllLocationsAsync();

            using var assertContext = new StoreContext(options);
            Assert.NotNull(locationList);
            Assert.Equal(2, locationList.Count);
        }

        [Fact]
        public void GetLocationByIdShouldGetLocationById()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("GetLocationByIdShouldGetLocationById").Options;
            using var testContext = new StoreContext(options);
            locationRepo = new LocationRepo(testContext);
            Seed(testContext);

            Location location = locationRepo.GetLocationById(1);

            using var assertContext = new StoreContext(options);
            Assert.NotNull(location);
            Assert.Equal("Location 1", location.Name);
        }

        [Fact]
        public async void GetAllProductsShouldGetAllProducts()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("GetAllProductsShouldGetAllProducts").Options;
            using var testContext = new StoreContext(options);
            productRepo = new ProductRepo(testContext);
            Seed(testContext);

            List<Product> productList = await productRepo.GetAllProductsAsync();

            using var assertContext = new StoreContext(options);
            Assert.NotNull(productList);
            Assert.Equal(8, productList.Count);
        }

        [Fact]
        public void GetProductByIdShouldGetProductById()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("GetProductByIdShouldGetProductById").Options;
            using var testContext = new StoreContext(options);
            productRepo = new ProductRepo(testContext);
            Seed(testContext);

            Product product = productRepo.GetProductById(1);

            using var assertContext = new StoreContext(options);
            Assert.NotNull(product);
            Assert.Equal("Solar Panel", product.Name);
        }

        [Fact]
        public async void GetProductStockByLocationShouldGetProductStockByLocation()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("GetProductStockByLocationShouldGetProductStockByLocation").Options;
            using var testContext = new StoreContext(options);
            productRepo = new ProductRepo(testContext);
            Seed(testContext);

            List<ProductStock> productStockList = await productRepo.GetProductStockByLocation(1);

            using var assertContext = new StoreContext(options);
            Assert.NotNull(productStockList);
            Assert.Equal(2, productStockList.Count);
        }

        [Fact]
        public async void GetProductStockByProductIdShouldGetProductStockByProductId()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("GetProductStockByProductIdShouldGetProductStockByProductId").Options;
            using var testContext = new StoreContext(options);
            productRepo = new ProductRepo(testContext);
            Seed(testContext);

            List<ProductStock> productStockList = await productRepo.GetProductStockByProductId(1);

            using var assertContext = new StoreContext(options);
            Assert.NotNull(productStockList);
            Assert.Equal(1, productStockList.Count);
        }

        [Fact]
        public void AddOrderShouldAddOrder()
        {
            var options = new DbContextOptionsBuilder<StoreContext>().UseInMemoryDatabase("AddOrderShouldAddOrder").Options;
            using var testContext = new StoreContext(options);
            orderRepo = new OrderRepo(testContext);

            testCart.Add(1, 1);
            testCart.Add(2, 1);

            orderRepo.AddOrder(testOrder, testCart);

            using var assertContext = new StoreContext(options);
            Assert.NotNull(assertContext.Orders.Single(o => o.OrderId == testOrder.OrderId));
        }
    }
}
