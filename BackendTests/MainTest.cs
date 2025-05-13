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
            Console.WriteLine("we check achya's kuku requirements");
            Console.WriteLine("🚀 Running all tests!\n");
            Console.WriteLine("👤 Running user-related tests...");
            new RegistrationTests().RunAll();
            Console.WriteLine("👤 Running user-related tests...");
            new LoginTests().RunAll();
            Console.WriteLine("\n🧠 Running board-related tests...");
            new BoardTests().RunAll();
            Console.WriteLine("\n📋 Running task-related tests...");
            new TaskTests().RunAll();
            Console.WriteLine("\n✅ All test suites finished.");
        }
    }
}