
namespace MIcrosoftGraphService
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Starting MIcrosoftGraphService...");
            var s = new Server();
            if (!s.Start(args))
            {
                Environment.Exit(1);
            }
        }
    }
}