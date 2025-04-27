using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.ServiceLayer;

namespace Tests
{
    public class RegistrationTests
    {
        private ServiceFactory _factory;

        /// <summary>
        /// Test registering with an already existing email.
        /// Preconditions: The email already exists in the system.
        /// Postconditions: The registration fails and returns an error.
        /// Throws: None.
        /// </summary>
        public bool TestExistedEmailRegister()
        {
            _factory.Us.Register("testUser", "Password1", "existing@email.com");
            Response response = _factory.Us.Register("existingUser", "Password2", "existing@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test registering without a username.
        /// Preconditions: Username is null.
        /// Postconditions: The registration fails and returns an error.
        /// Throws: None.
        /// </summary>
        public bool TestNoUsernameRegister()
        {
            Response response = _factory.Us.Register(null, "Password1", "new@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Us.Register("user", "Password1", "invalidemail");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Us.Register("newUser", "Password1", "new@email.com");
            if (response.ErrorMsg != null || response.RetVal == null)
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
            Response response = _factory.Us.Register("user", "123", "short@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Us.Register("user", "Password!123456789010", "long@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Us.Register("user", "password1", "noupper@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Us.Register("user", "PASSWORD1", "nolower@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Us.Register("user", "Password", "nodigit@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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