using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.BackendTests.Testings;

namespace IntroSE.Kanban.BackendTests
{
    public class MainTest
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("🔧 Initializing test environment...");
            ServiceFactory serviceFactory = new();
            serviceFactory.GetBoardService().DeleteData();
            Console.WriteLine("\n🚀 Running all tests!\n");
            Console.WriteLine("👤 Running registration-related tests...");
            new RegistrationTests().RunAll(serviceFactory);
            Console.WriteLine("\n👤 Running login-related tests...");
            new LoginTests().RunAll(serviceFactory);
            Console.WriteLine("\n🧠 Running board-related tests...");
            new BoardTests().RunAll(serviceFactory);
            Console.WriteLine("\n📋 Running task-related tests...");
            new TaskTests().RunAll(serviceFactory);
        }
    }
}