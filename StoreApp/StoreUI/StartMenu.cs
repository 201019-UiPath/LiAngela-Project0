using System;

using StoreDB;
using StoreDB.Repos;

namespace StoreUI
{
    public class StartMenu : IMenu
    {
        private string userInput;

        private CustomerMenu customerMenu;

        private ManagerMenu managerMenu;

        public StartMenu(StoreContext context)
        {
            this.customerMenu = new CustomerMenu(new CustomerRepo(context));
            this.managerMenu = new ManagerMenu(new CustomerRepo(context));
        }

        public void Start()
        {
            do {
                Console.WriteLine("\nWelcome to Li's Home Energy Efficiency Improvement!");
                Console.WriteLine("\nWe strive in every way to fill the market niche created by Lowe's Home Improvement's energy-inefficient product offerings, and that is why all of our locations are right across the street from a Lowe's!");
                Console.Write("\nAre you a customer [0] or the manager [1]? (type \"x\" to exit) ");
                userInput = Console.ReadLine();
                switch (userInput) {
                    case "0":
                        customerMenu.Start();
                        break;
                    case "1":
                        managerMenu.Start();
                        break;
                }
            } while (!userInput.Equals("x"));
        }
    }
}