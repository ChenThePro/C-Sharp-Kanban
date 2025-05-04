using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class RegistrationTests
    {
        private ServiceFactory _factory = new ServiceFactory(new BoardFacade(), new UserFacade());

        /// <summary>
        /// Test registering with an already existing email.
        /// Preconditions: The email already exists in the system.
        /// Postconditions: The registration fails and returns an error.
        /// Throws: None.
        /// </summary>
        public bool TestExistedEmailRegister()
        {
            _factory.GetUserService().Register("existing@email.com", "Password1");
            Response<UserSL> response = _factory.GetUserService().Register("existing@email.com", "Password2");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test registering without an email.
        /// Preconditions: Email is null.
        /// Postconditions: The registration fails and returns an error.
        /// Throws: None.
        /// </summary>
        public bool TestNoUsernameRegister()
        {
            Response<UserSL> response = _factory.GetUserService().Register(null, "Password1");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test registering with an invalid email format.
        /// Preconditions: Email format is incorrect.
        /// Postconditions: The registration fails and returns an error.
        /// Throws: None.
        /// </summary>
        public bool TestInvalidEmailRegister()
        {
            Response<UserSL> response = _factory.GetUserService().Register("invalidemail", "Password1");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test successful registration.
        /// Preconditions: All parameters are valid.
        /// Postconditions: Registration succeeds.
        /// Throws: None.
        /// </summary>
        public bool TestSuccessfullyRegister()
        {
            Response<UserSL> response = _factory.GetUserService().Register("new@email.com", "Password1");
            if (response.RetVal == null)
                return false;
            return true;
        }

        /// <summary>
        /// Test registering with a short password.
        /// Preconditions: Password is less than required length.
        /// Postconditions: Registration fails.
        /// Throws: None.
        /// </summary>
        public bool TestShortPasswordRegister()
        {
            Response<UserSL> response = _factory.GetUserService().Register("short@email.com", "123");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test registering with a long password.
        /// Preconditions: Password exceeds maximum length.
        /// Postconditions: Registration fails.
        /// Throws: None.
        /// </summary>
        public bool TestLongPasswordRegister()
        {
            Response<UserSL> response = _factory.GetUserService().Register("long@email.com", "Password!123456789010");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test registering with a password missing uppercase characters.
        /// Preconditions: Password lacks uppercase letters.
        /// Postconditions: Registration fails.
        /// Throws: None.
        /// </summary>
        public bool TestNonUpperPasswordRegister()
        {
            Response<UserSL> response = _factory.GetUserService().Register("noupper@email.com", "password1");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test registering with a password missing lowercase characters.
        /// Preconditions: Password lacks lowercase letters.
        /// Postconditions: Registration fails.
        /// Throws: None.
        /// </summary>
        public bool TestNonLowercasePasswordRegister()
        {
            Response<UserSL> response = _factory.GetUserService().Register("nolower@email.com", "PASSWORD1");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test registering with a password missing digits.
        /// Preconditions: Password lacks numeric characters.
        /// Postconditions: Registration fails.
        /// Throws: None.
        /// </summary>
        public bool TestNonDigitsPasswordRegister()
        {
            Response<UserSL> response = _factory.GetUserService().Register("nodigit@email.com", "Password");
            if (response.RetVal != null)
                return false;
            return true;
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
