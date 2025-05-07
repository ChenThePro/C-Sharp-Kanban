using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
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
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestWrongUsernameLogin()
        {
            string json = _factory.GetUserService().Login("wrongUser@gmail.com", _password);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestWrongPasswordLogin()
        {
            string json = _factory.GetUserService().Login(_userEmail, "wrongPassword");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestLogout()
        {
            string json = _factory.GetUserService().Logout(_userEmail);
            return json.Contains("\"ErrorMessage\":null");
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
