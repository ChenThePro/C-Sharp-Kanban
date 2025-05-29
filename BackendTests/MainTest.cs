using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.BackendTests.Testings;
using log4net;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.BackendTests
{
    public class MainTest
    {
        public static void Main(string[] args)
        {
            // new ServiceFactory().GetBoardService().DeleteData();
            // return;
            // SQLitePCL.Batteries_V2.Init();
            ServiceFactory serviceFactory = new ServiceFactory();
            Console.WriteLine("🚀 Running all tests!\n");
            Console.WriteLine("👤 Running registration-related tests...");
            new RegistrationTests().RunAll(serviceFactory);
            Console.WriteLine("\n👤 Running login-related tests...");
            new LoginTests().RunAll(serviceFactory);
            Console.WriteLine("\n🧠 Running board-related tests...");
            new BoardTests().RunAll(serviceFactory);
            Console.WriteLine("\n📋 Running task-related tests...");
            new TaskTests().RunAll(serviceFactory);
            Console.WriteLine("\n✅ All test suites finished.");
        }
    }
}