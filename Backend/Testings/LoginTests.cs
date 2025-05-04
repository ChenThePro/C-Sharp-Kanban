using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.ServiceLayer;

namespace Tests
{
    public class LoginTests
    {
        private ServiceFactory _factory;

        /// <summary>
        /// Test login after successful registration.
        /// Preconditions: A user is successfully registered.
        /// Postconditions: Login should succeed with correct credentials.
        /// Throws: None.
        /// </summary>
        public bool TestLoggedInAfterRegister()
        {
            _factory.Us.Register("user", "Password1", "user@email.com");
            Response response = _factory.Us.Login("user", "Password1");
            if (response.ErrorMsg != null || response.RetVal == null)
                return false;
            return true;
        }

        /// <summary>
        /// Test login with valid credentials.
        /// Preconditions: Credentials are correct and user is registered.
        /// Postconditions: Login should succeed.
        /// Throws: None.
        /// </summary>
        public bool TestLogin()
        {
            Response response = _factory.Us.Login("validUser", "Password1");
            if (response.ErrorMsg != null || response.RetVal == null)
                return false;
            return true;
        }

        /// <summary>
        /// Test login with invalid username.
        /// Preconditions: Username does not exist.
        /// Postconditions: Login should fail with error.
        /// Throws: None.
        /// </summary>
        public bool TestWrongUsernameLogin()
        {
            Response response = _factory.Us.Login("wrongUser", "Password1");
            if (response.ErrorMsg == null || response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test login with wrong password.
        /// Preconditions: Password is incorrect for given username.
        /// Postconditions: Login should fail.
        /// Throws: None.
        /// </summary>
        public bool TestWrongPasswordLogin()
        {
            Response response = _factory.Us.Login("validUser", "wrongPassword");
            if (response.ErrorMsg == null || response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test logout after successful login.
        /// Preconditions: User is logged in.
        /// Postconditions: Logout should succeed.
        /// Throws: None.
        /// </summary>
        public bool TestLogout()
        {
            Response response = _factory.Us.Logout();
            if (response.ErrorMsg != null || response.RetVal == null)
                return false;
            return true;
        }

        public void RunAll()
        {
            Console.WriteLine("🔹 TestLoggedInAfterRegister: " + TestLoggedInAfterRegister());
            Console.WriteLine("🔹 TestLogin: " + TestLogin());
            Console.WriteLine("🔹 TestWrongUsernameLogin: " + TestWrongUsernameLogin());
            Console.WriteLine("🔹 TestWrongPasswordLogin: " + TestWrongPasswordLogin());
            Console.WriteLine("🔹 TestLogout: " + TestLogout());
        }
    }
}
