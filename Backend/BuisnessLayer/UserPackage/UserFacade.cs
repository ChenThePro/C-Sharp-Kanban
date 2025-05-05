using log4net;
using System.Text.RegularExpressions;

namespace Backend.BuisnessLayer.UserPackage
{
    internal class UserFacade
    {
        private Dictionary<string, UserBL> _emails;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal UserFacade()
        {
            _emails = new Dictionary<string, UserBL>();
        }

        /// <summary>
        /// Attempts to log in a user with the provided email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The <see cref="UserBL"/> instance associated with the user.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the email does not exist.</exception>
        internal UserBL Login(string email, string password)
        {
            if (_emails.ContainsKey(email))
                return _emails[email].Login(password);
            throw new KeyNotFoundException("email doesn't exist");
        }

        /// <summary>
        /// Logs out the user associated with the given email.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        internal void Logout(string email)
        {
            _emails[email].Logout();
        }

        /// <summary>
        /// Registers a new user with the provided email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The newly created <see cref="UserBL"/> instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the email already exists.</exception>
        /// <exception cref="FormatException">Thrown if the email format is invalid.</exception>
        /// <exception cref="ArgumentException">Thrown if the password does not meet complexity requirements.</exception>
        internal UserBL Register(string email, string password)
        {
            if (_emails.ContainsKey(email))
                throw new InvalidOperationException("Email already exists");
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new FormatException("Email format is not valid");
            if (password.Length < 6 || password.Length > 20)
                throw new ArgumentException("Password must be between 6 and 20 characters");
            if (!Regex.IsMatch(password, @"[A-Z]"))
                throw new ArgumentException("Password must contain at least one uppercase letter");
            if (!Regex.IsMatch(password, @"[a-z]"))
                throw new ArgumentException("Password must contain at least one lowercase letter");
            if (!Regex.IsMatch(password, @"\d"))
                throw new ArgumentException("Password must contain at least one number");
            UserBL newUser = new UserBL(email, password);
            _emails[email] = newUser;
            Log.Info("new user" + email + "created");
            return newUser;
        }
    }
}