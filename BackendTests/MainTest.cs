using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.BackendTests.Testings;

namespace IntroSE.Kanban.BackendTests
{
    public class MainTest
    {
        public static void Main(string[] args)
        {
            // SQLitePCL.Batteries_V2.Init();
            ServiceFactory serviceFactory = new();
            // serviceFactory.GetBoardService().DeleteData(); return;
            Console.WriteLine("🚀 Running all tests!\n");
            Console.WriteLine("👤 Running registration-related tests...");
            new RegistrationTests().RunAll(serviceFactory);
            Console.WriteLine("\n👤 Running login-related tests...");
            new LoginTests().RunAll(serviceFactory);
            Console.WriteLine("\n🧠 Running board-related tests...");
            new BoardTests().RunAll(serviceFactory);
            Console.WriteLine("\n📋 Running task-related tests...");
            new TaskTests().RunAll(serviceFactory);
            Console.WriteLine("\n💾 Running persistence-related tests...");
        }
    }
}