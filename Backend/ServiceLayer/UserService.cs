using System;
using System.Text.Json;
using IntroSE.Kanban.Backend.BuisnessLayer.UserPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private readonly UserFacade _userFacade;

        /// <summary>
        /// Constructs a new UserService with the given UserFacade.
        /// </summary>
        /// <param name="userFacade">The facade to handle user-related operations.</param>
        internal UserService(UserFacade userFacade)
        {
            _userFacade = userFacade;
        }

        private string ToJsonResponse(string error = null, object data = null) =>
            JsonSerializer.Serialize(new Response(error, data));

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="email">User's email address.</param>
        /// <param name="password">User's password.</param>
        /// <returns>JSON-serialized Response containing the logged-in UserSL object or an error message.</returns>
        /// <exception cref="UnauthorizedAccessException">If the password is incorrect.</exception>
        /// <exception cref="KeyNotFoundException">If the email does not exist.</exception>
        /// <precondition>The email must exist, and the password must match the stored credentials.</precondition>
        /// <postcondition>The user is marked as logged in if credentials are valid.</postcondition>
        public string Login(string email, string password)
        {
            try
            {
                _userFacade.Login(email, password);
                return ToJsonResponse(null, email);
            }
            catch (Exception ex)
            {
                return ToJsonResponse(ex.Message);
            }
        }

        /// <summary>
        /// Registers a new user with the given credentials.
        /// </summary>
        /// <param name="email">Desired email address.</param>
        /// <param name="password">Desired password.</param>
        /// <returns>JSON-serialized Response containing the newly created UserSL object or an error message.</returns>
        /// <exception cref="InvalidOperationException">If the email is already in use.</exception>
        /// <exception cref="FormatException">If the email format is invalid.</exception>
        /// <exception cref="ArgumentException">If the password does not meet requirements.</exception>
        /// <precondition>Email must be unique and properly formatted; password must meet complexity rules.</precondition>
        /// <postcondition>The new user is added to the system and marked as logged in.</postcondition>
        public string Register(string email, string password)
        {
            try
            {
                _userFacade.Register(email, password);
                return ToJsonResponse();
            }
            catch (Exception ex)
            {
                return ToJsonResponse(ex.Message);
            }
        }

        /// <summary>
        /// Logs out the user with the specified email.
        /// </summary>
        /// <param name="email">User's email address.</param>
        /// <returns>JSON-serialized empty Response object or an error message.</returns>
        /// <exception cref="InvalidOperationException">If the user is not currently logged in.</exception>
        /// <exception cref="KeyNotFoundException">If the email does not exist.</exception>
        /// <precondition>The user must exist and be currently logged in.</precondition>
        /// <postcondition>The user is marked as logged out.</postcondition>
        public string Logout(string email)
        {
            try
            {
                _userFacade.Logout(email);
                return ToJsonResponse();
            }
            catch (Exception ex)
            {
                return ToJsonResponse(ex.Message);
            }
        }
    }
}