using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class LoginTests
    {
        private ServiceFactory _factory = new ServiceFactory();

        /// <summary>
        /// Test login after successful registration.
        /// Preconditions: A user is successfully registered.
        /// Postconditions: Login should succeed with correct credentials.
        /// Throws: None.
        /// </summary>
        public bool TestLoggedInAfterRegister()
        {
            _factory.GetUserService().Register("user@email.com", "Password1");
            Response<UserSL> response = JsonSerializer.Deserialize<Response<UserSL>>(_factory.GetUserService().Login("user@email.com", "Password1"));
            return response.ErrorMsg != null;
        }

        /// <summary>
        /// Test login with invalid email.
        /// Preconditions: Username does not exist.
        /// Postconditions: Login should fail with error.
        /// Throws: None.
        /// </summary>
        public bool TestWrongUsernameLogin()
        {
            Response<UserSL> response = JsonSerializer.Deserialize<Response<UserSL>>(_factory.GetUserService().Login("wrongUser@gmail.com", "Password1"));
            return response.ErrorMsg == null;
        }

        /// <summary>
        /// Test login with wrong password.
        /// Preconditions: Password is incorrect for given username.
        /// Postconditions: Login should fail.
        /// Throws: None.
        /// </summary>
        public bool TestWrongPasswordLogin()
        {
            Response<UserSL> response = JsonSerializer.Deserialize<Response<UserSL>>(_factory.GetUserService().Login("user@email.com", "wrongPassword"));
            return response.ErrorMsg == null;
        }

        /// <summary>
        /// Test logout after successful login.
        /// Preconditions: User is logged in.
        /// Postconditions: Logout should succeed.
        /// Throws: None.
        /// </summary>
        public bool TestLogout()
        {
            Response<object> response = JsonSerializer.Deserialize<Response<object>>(_factory.GetUserService().Logout("user@email.com"));
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
