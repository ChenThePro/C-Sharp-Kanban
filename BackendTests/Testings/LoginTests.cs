using System.Text.Json;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class LoginTests
    {
        private readonly ServiceFactory _factory = new ServiceFactory();

        public bool TestLoggedInAfterRegister()
        {
            _factory.GetUserService().Register("user@email.com", "Password1");
            var json = _factory.GetUserService().Login("user@email.com", "Password1");
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg == null;
        }

        public bool TestWrongUsernameLogin()
        {
            var json = _factory.GetUserService().Login("wrongUser@gmail.com", "Password1");
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg != null;
        }

        public bool TestWrongPasswordLogin()
        {
            var json = _factory.GetUserService().Login("user@email.com", "wrongPassword");
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg != null;
        }

        public bool TestLogout()
        {
            var json = _factory.GetUserService().Logout("user@email.com");
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg == null;
        }

        public void RunAll()
        {
            Console.WriteLine("🔹 TestLoggedInAfterRegister: " + TestLoggedInAfterRegister());
            Console.WriteLine("🔹 TestWrongUsernameLogin: " + TestWrongUsernameLogin());
            Console.WriteLine("🔹 TestWrongPasswordLogin: " + TestWrongPasswordLogin());
            Console.WriteLine("🔹 TestLogout: " + TestLogout());
        }
    }
}
