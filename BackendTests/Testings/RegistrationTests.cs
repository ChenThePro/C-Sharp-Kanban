using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
{
    public class RegistrationTests
    {
        private readonly ServiceFactory _factory = new ServiceFactory();

        public bool TestExistedEmailRegister()
        {
            _factory.GetUserService().Register("existing@email.com", "Password1");
            string json = _factory.GetUserService().Register("existing@email.com", "Password2");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestNoUsernameRegister()
        {
            string json = _factory.GetUserService().Register(null!, "Password1");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestInvalidEmailRegister()
        {
            string json = _factory.GetUserService().Register("invalidemail", "Password1");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestSuccessfullyRegister()
        {
            string json = _factory.GetUserService().Register("new@email.com", "Password1");
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool TestShortPasswordRegister()
        {
            string json = _factory.GetUserService().Register("short@email.com", "123");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestLongPasswordRegister()
        {
            string json = _factory.GetUserService().Register("long@email.com", "Password!123456789010");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestNonUpperPasswordRegister()
        {
            string json = _factory.GetUserService().Register("noupper@email.com", "password1");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestNonLowercasePasswordRegister()
        {
            string json = _factory.GetUserService().Register("nolower@email.com", "PASSWORD1");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestNonDigitsPasswordRegister()
        {
            string json = _factory.GetUserService().Register("nodigit@email.com", "Password");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public void RunAll()
        {
            Console.WriteLine("🔹 TestExistedEmailRegister: " + TestExistedEmailRegister());
            Console.WriteLine("🔹 TestNoUsernameRegister: " + TestNoUsernameRegister());
            Console.WriteLine("🔹 TestInvalidEmailRegister: " + TestInvalidEmailRegister());
            Console.WriteLine("🔹 TestSuccessfullyRegister: " + TestSuccessfullyRegister());
            Console.WriteLine("🔹 TestShortPasswordRegister: " + TestShortPasswordRegister());
            Console.WriteLine("🔹 TestLongPasswordRegister: " + TestLongPasswordRegister());
            Console.WriteLine("🔹 TestNonUpperPasswordRegister: " + TestNonUpperPasswordRegister());
            Console.WriteLine("🔹 TestNonLowercasePasswordRegister: " + TestNonLowercasePasswordRegister());
            Console.WriteLine("🔹 TestNonDigitsPasswordRegister: " + TestNonDigitsPasswordRegister());
        }
    }
}
