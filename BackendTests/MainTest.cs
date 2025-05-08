using IntroSE.Kanban.Backend.ServiceLayer;
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

            GradingService noa = new GradingService();
            Console.WriteLine(noa.Register("stringNe@gmail.com", "Aa123456"));
            Console.WriteLine(noa.CreateBoard("stringNe@gmail.com", "tasks"));
            Console.WriteLine(noa.AddTask("stringNe@gmail.com", "tasks", "1", "cjnc", DateTime.Now));
            Console.WriteLine(noa.AddTask("stringNe@gmail.com", "tasks", "2", "cjnc", DateTime.Now));
            Console.WriteLine(noa.GetColumn("stringNe@gmail.com", "tasks", 0));
            Console.WriteLine(noa.UpdateTaskDescription("stringNe@gmail.com", "tasks", 0, 1, "jdk"));
            Console.WriteLine(noa.GetColumn("stringNe@gmail.com", "tasks", 0));
        }
    }
}