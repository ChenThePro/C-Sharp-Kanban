using System.Text.Json;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class RegistrationTests
    {
        private readonly ServiceFactory _factory = new ServiceFactory();

        public bool TestExistedEmailRegister()
        {
            _factory.GetUserService().Register("existing@email.com", "Password1");
            string json = _factory.GetUserService().Register("existing@email.com", "Password2");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestNoUsernameRegister()
        {
            string json = _factory.GetUserService().Register(null!, "Password1");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestInvalidEmailRegister()
        {
            string json = _factory.GetUserService().Register("invalidemail", "Password1");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestSuccessfullyRegister()
        {
            string json = _factory.GetUserService().Register("new@email.com", "Password1");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg == null;
        }

        public bool TestShortPasswordRegister()
        {
            string json = _factory.GetUserService().Register("short@email.com", "123");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestLongPasswordRegister()
        {
            string json = _factory.GetUserService().Register("long@email.com", "Password!123456789010");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestNonUpperPasswordRegister()
        {
            string json = _factory.GetUserService().Register("noupper@email.com", "password1");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestNonLowercasePasswordRegister()
        {
            string json = _factory.GetUserService().Register("nolower@email.com", "PASSWORD1");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool TestNonDigitsPasswordRegister()
        {
            string json = _factory.GetUserService().Register("nodigit@email.com", "Password");
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
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
