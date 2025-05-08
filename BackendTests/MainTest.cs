using IntroSE.Kanban.BackendTests.Testings;

namespace IntroSE.Kanban.BackendTests
{
    public class MainTest
    {
        public static void Main(string[] args)
        {
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