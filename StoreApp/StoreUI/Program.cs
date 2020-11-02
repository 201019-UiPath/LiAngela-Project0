using StoreDB;

namespace StoreUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // add logging
            IMenu startMenu = new StartMenu(new StoreContext());
            startMenu.Start();
        }
    }
}
