using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
{
    public class RegistrationTests
    {
        private ServiceFactory _factory = null!;
        private const string EMAIL1 = "test1@gmail.com";
        private const string EMAIL2 = "test2@gmail.com";
        private const string PASSWORD = "Password1";

        public bool TestExistedEmailRegister()
        {
            _factory.GetUserService().Register(EMAIL1, PASSWORD);
            string json = _factory.GetUserService().Register(EMAIL1, PASSWORD);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestNoUsernameRegister()
        {
            string json = _factory.GetUserService().Register(null, PASSWORD);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestInvalidEmailRegister()
        {
            string json = _factory.GetUserService().Register("invalidemail", PASSWORD);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestSuccessfullyRegister()
        {
            string json = _factory.GetUserService().Register(EMAIL2, PASSWORD);
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

        public void RunAll(ServiceFactory serviceFactory)
        {
            _factory = serviceFactory;
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