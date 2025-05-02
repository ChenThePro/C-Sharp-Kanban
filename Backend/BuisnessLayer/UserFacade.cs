using Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Backend.BuisnessLayer
{
    internal class UserFacade
    {
        private Dictionary<string, UserBL> _emails;

        internal UserFacade()
        {
            _emails = new Dictionary<string, UserBL>();
        }

        internal UserBL Login(string email, string password)
        {
            if (_emails.ContainsKey(email))
                return _emails[email].Login(password);
            throw new KeyNotFoundException("email doesn't exist");
        }

        internal void Logout(string email)
        {
            _emails[email].Logout();
        }

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
            return newUser;
        }
    }
}