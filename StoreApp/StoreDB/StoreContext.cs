using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

using StoreDB.Models;

namespace StoreDB
{
    public class StoreContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        
        public DbSet<ProductStock> ProductStocks { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var connectionString = configuration.GetConnectionString("StoreDB");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Order>()
            .HasOne(e => e.Customer)
            .WithMany(c => c.OrderHistory)
            .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<Order>()
            .HasOne(e => e.Location)
            .WithMany(l => l.OrderHistory)
            .HasForeignKey(e => e.LocationId);

            modelBuilder.Entity<Order>()
            .Property(e => e.OrderId)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderItem>()
            .HasOne(e => e.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<OrderItem>()
            .HasOne(e => e.Product)
            .WithMany(p => p.ProductOrders)
            .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<ProductStock>()
            .HasOne(e => e.Location)
            .WithMany(l => l.ProductStocks)
            .HasForeignKey(e => e.LocationId);

            modelBuilder.Entity<ProductStock>()
            .HasOne(e => e.Product)
            .WithMany(p => p.ProductStocks)
            .HasForeignKey(e => e.ProductId);

            // seed data
            modelBuilder.Entity<Customer>().HasData(
                new Customer {CustomerId = 1, Name = "Me", PhoneNumber = "(123) 456 7890", EmailAddress = "me@internet.net", MailingAddress = "111 Residential St"});
            
            modelBuilder.Entity<Location>().HasData(
                new Location {LocationId = 1, Name = "Location 1", PhoneNumber = "(111) 111 1111", Address = "111 Main St"},
                new Location {LocationId = 2, Name = "Location 2", PhoneNumber = "(211) 111 1111", Address = "211 Main St"},
                new Location {LocationId = 3, Name = "Location 3", PhoneNumber = "(311) 111 1111", Address = "311 Main St"});
            
            modelBuilder.Entity<Product>().HasData(
                // new Product {ProductId = 1, Name = "Solar Panel", Price = 100, Description = "It is a solar panel"},
                new Product {ProductId = 1, Name = "Solar Panel", Price = 100},
                new Product {ProductId = 2, Name = "Insulated Window", Price = 50},
                new Product {ProductId = 3, Name = "Energy-efficient Refrigerator", Price = 200},
                new Product {ProductId = 4, Name = "Energy-efficient Dishwasher", Price = 201},
                new Product {ProductId = 5, Name = "Energy-efficient Microwave", Price = 202},
                new Product {ProductId = 6, Name = "Energy-efficient Oven", Price = 203},
                new Product {ProductId = 7, Name = "High-efficiency Washer", Price = 204},
                new Product {ProductId = 8, Name = "High-efficiency Dryer", Price = 205},
                new Product {ProductId = 9, Name = "LED Outdoor Light Fixture", Price = 20},
                new Product {ProductId = 10, Name = "LED Indoor Light Fixture", Price = 20});
            
            modelBuilder.Entity<ProductStock>().HasData(
                new ProductStock {Id = 1, LocationId = 1, ProductId = 1, QuantityStocked = 10},
                new ProductStock {Id = 2, LocationId = 1, ProductId = 2, QuantityStocked = 23},
                new ProductStock {Id = 3, LocationId = 1, ProductId = 3, QuantityStocked = 10},
                new ProductStock {Id = 4, LocationId = 2, ProductId = 4, QuantityStocked = 10},
                new ProductStock {Id = 5, LocationId = 3, ProductId = 5, QuantityStocked = 10});
        }
    }
}