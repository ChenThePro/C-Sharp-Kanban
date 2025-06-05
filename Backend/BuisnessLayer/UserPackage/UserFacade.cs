using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BuisnessLayer.UserPackage
{
    internal class UserFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal readonly Dictionary<string, UserBL> _emails;
        private readonly Regex PasswordCharRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFacade"/> class.
        /// </summary>
        internal UserFacade()
        {
            _emails = new Dictionary<string, UserBL>();
            PasswordCharRegex = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?~` ]+$");
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                Log.Error("Email cannot be empty");
                throw new ArgumentNullException("Email cannot be empty");
            }
            if (_emails.ContainsKey(email))
            {
                Log.Error("Email already exists.");
                throw new InvalidOperationException("Email already exists.");
            }
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Log.Error("Email format is not valid.");
                throw new FormatException("Email format is not valid.");
            }
        }

        private void VaildatePassword(string password)
        {
            if (password.Length < 6 || password.Length > 20)
            {
                Log.Error("Password must be between 6 and 20 characters.");
                throw new ArgumentOutOfRangeException("Password must be between 6 and 20 characters.");
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                Log.Error("Password must contain at least one uppercase letter.");
                throw new ArgumentException("Password must contain at least one uppercase letter.");
            }
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                Log.Error("Password must contain at least one lowercase letter.");
                throw new ArgumentException("Password must contain at least one lowercase letter.");
            }
            if (!Regex.IsMatch(password, @"\d"))
            {
                Log.Error("Password must contain at least one number.");
                throw new ArgumentException("Password must contain at least one number.");
            }
        }

        internal void AuthenticateUser(string email)
        {
            if (!_emails.ContainsKey(email))
            {
                Log.Error("Email doesn't exist.");
                throw new KeyNotFoundException("Email doesn't exist.");
            }
        }

        /// <summary>
        /// Attempts to log in a user with the provided email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The <see cref="UserBL"/> instance representing the logged-in user.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the email does not exist.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the password is incorrect.</exception>
        /// <precondition>User must exist in the system.</precondition>
        /// <postcondition>User is marked as logged in if credentials are valid.</postcondition>
        internal UserBL Login(string email, string password)
        {
            AuthenticateUser(email);
            return _emails[email].Login(password);
        }

        /// <summary>
        /// Logs out the user associated with the given email.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the email does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user is already logged out.</exception>
        /// <precondition>User must be currently logged in.</precondition>
        /// <postcondition>User is marked as logged out.</postcondition>
        internal void Logout(string email)
        {
            AuthenticateUser(email);
            _emails[email].Logout();
        }

        /// <summary>
        /// Registers a new user with the provided email and password.
        /// </summary>
        /// <param name="email">The desired email address.</param>
        /// <param name="password">The desired password.</param>
        /// <returns>The newly created <see cref="UserBL"/> instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the email already exists.</exception>
        /// <exception cref="FormatException">Thrown if the email format is invalid.</exception>
        /// <exception cref="ArgumentException">Thrown if the password does not meet complexity requirements.</exception>
        /// <precondition>Email must be unique and valid. Password must meet complexity rules.</precondition>
        /// <postcondition>User is added to the system and marked as logged in.</postcondition>
        internal UserBL Register(string email, string password)
        {
            ValidateEmail(email);
            VaildatePassword(password);
            UserBL newUser = new UserBL(email, password);
            _emails[email] = newUser;
            Log.Info("New user " + email + " created.");
            return newUser;
        }

        internal UserBL GetUser(string email)
        {
            return _emails[email];
        }
        public void LoadData()
        {
            List<UserDTO> users = new UserDTO().SelectAll();
            UserBL user;
            foreach (UserDTO userDTO in users)
            {
                user = new UserBL(userDTO);
                _emails.Add(user.Email, user);
            }
        }
    }
}