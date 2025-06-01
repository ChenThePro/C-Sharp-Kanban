using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
{
    public class LoginTests
    {
        private ServiceFactory _factory = null!;
        private const string EMAIL1 = "test1@gmail.com";
        private const string EMAIL2 = "test2@gmail.com";
        private const string PASSWORD = "Password1";

        public bool TestLoggedInAfterRegister()
        {
            string json = _factory.GetUserService().Login(EMAIL1, PASSWORD);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestWrongUsernameLogin()
        {
            string json = _factory.GetUserService().Login("wrongUser@gmail.com", PASSWORD);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestWrongPasswordLogin()
        {
            string json = _factory.GetUserService().Login(EMAIL1, "wrongPassword");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestLogout()
        {
            string json = _factory.GetUserService().Logout(EMAIL2);
            return json.Contains("\"ErrorMessage\":null");
        }

        public void RunAll(ServiceFactory serviceFactory)
        {
            _factory = serviceFactory;
            Console.WriteLine("🔹 TestLoggedInAfterRegister: " + TestLoggedInAfterRegister());
            Console.WriteLine("🔹 TestWrongUsernameLogin: " + TestWrongUsernameLogin());
            Console.WriteLine("🔹 TestWrongPasswordLogin: " + TestWrongPasswordLogin());
            Console.WriteLine("🔹 TestLogout: " + TestLogout());
        }
    }
}