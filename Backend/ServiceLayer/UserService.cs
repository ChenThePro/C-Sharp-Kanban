using System.Text.Json;
using Backend.BuisnessLayer.UserPackage;

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
        /// <param name="password">User's password.</param>
        /// <returns>Response containing the logged-in UserSL object.</returns>
        /// <exception cref="UnauthorizedAccessException">If the login fails.</exception>
        /// <exception cref="KeyNotFoundException">If the email doesn't exist.</exception>
        /// <precondition>The email and password are registered.</precondition>
        /// <postcondition>The user is marked as logged in.</postcondition>
        public string Login(string email, string password)
        {
            try
            {
                UserBL user = _userFacade.Login(email, password);
                return JsonSerializer.Serialize(new Response<UserSL>(null, new UserSL(user)));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response<UserSL>(ex.Message, null));
            }
        }

        /// <summary>
        /// Registers a new user with the given credentials and email.
        /// </summary>
        /// <param name="password">Desired password.</param>
        /// <param name="email">Email address.</param>
        /// <returns>Response containing the created UserSL object.</returns>
        /// <exception cref="InvalidOperationException">If the email is already taken.</exception>
        /// <exception cref="FormatException">If the email is not valid.</exception>
        /// <exception cref="ArgumentException">If the password does not meet it's requirements.</exception>
        /// <precondition>The email must be unique and valid.</precondition>
        /// <postcondition>A new user is added to the system.</postcondition>
        public string Register(string email, string password)
        {
            try
            {
                UserBL user = _userFacade.Register(email, password);
                return JsonSerializer.Serialize(new Response<UserSL>("Registerd successfuly", new UserSL(user)));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response<UserSL>(ex.Message, null));
            }
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>An empty Response indicating logout status.</returns>
        /// <precondition>A user must be currently logged in.</precondition>
        /// <postcondition>The user's session is invalidated.</postcondition>
        public string Logout(string email)
        {
            _userFacade.Logout(email);
            return JsonSerializer.Serialize(new Response<object>("Logged out", null));
        }

        internal string InProgressTasks(string email)
        {
            throw new NotImplementedException();
        }
    }
}