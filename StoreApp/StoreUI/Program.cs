using Serilog;
using StoreDB;

namespace StoreUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("log.txt")
            .CreateLogger();

            Log.Information("Program started");

            IMenu startMenu = new StartMenu(new StoreContext());
            startMenu.Start();

            Log.CloseAndFlush();
        }
    }
}
