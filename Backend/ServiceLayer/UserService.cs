 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;

namespace Backend.ServiceLayer
{
    public class UserService
    {
        private readonly UserFacade _userFacade;

        internal UserService(UserFacade userFacade)
        {
            _userFacade = userFacade;
        }

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="username">User's username.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Response containing the logged-in UserSL object.</returns>
        /// <exception cref="UnauthorizedAccessException">If the login fails.</exception>
        /// <precondition>The username and password are registered.</precondition>
        /// <postcondition>The user is marked as logged in.</postcondition>
        public Response<UserSL> Login(string username, string password)
        {
            try
            {
                UserSL user = _userFacade.Login(username, password);
                return new Response<UserSL>("logged in :)", user);
            }
            catch (UnauthorizedAccessException)
            {
                return new Response<UserSL>("username or password incorrect", null);
            }
        }

        /// <summary>
        /// Registers a new user with the given credentials and email.
        /// </summary>
        /// <param name="username">Desired username.</param>
        /// <param name="password">Desired password.</param>
        /// <param name="email">Email address.</param>
        /// <returns>Response containing the created UserSL object.</returns>
        /// <exception cref="ArgumentException">If the username is already taken.</exception>
        /// <precondition>The username must be unique and email must be valid.</precondition>
        /// <postcondition>A new user is added to the system.</postcondition>
        public Response<UserSL> Register(string username, string password, string email)
        {
            try
            {
                UserSL user = _userFacade.Register(username, password, email);
                return new Response<UserSL>("", user);
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>An empty Response indicating logout status.</returns>
        /// <precondition>A user must be currently logged in.</precondition>
        /// <postcondition>The user's session is invalidated.</postcondition>
        public Response<object> Logout()
        {
            _userFacade.Logout();
            return new Response<object>("logout succusfully", null);
        }
    }
}
