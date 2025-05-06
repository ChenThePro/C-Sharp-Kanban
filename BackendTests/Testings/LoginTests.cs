using System.Text.Json;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class LoginTests
    {
        private readonly ServiceFactory _factory = new ServiceFactory();
        private string _userEmail = "user@email.com";
        private string _password = "Password1";

        public bool TestLoggedInAfterRegister()
        {
            _factory.GetUserService().Register(_userEmail, _password);
            string json = _factory.GetUserService().Login(_userEmail, _password);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestWrongUsernameLogin()
        {
            string json = _factory.GetUserService().Login("wrongUser@gmail.com", _password);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestWrongPasswordLogin()
        {
            string json = _factory.GetUserService().Login(_userEmail, "wrongPassword");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestLogout()
        {
            string json = _factory.GetUserService().Logout(_userEmail);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg == null;
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
