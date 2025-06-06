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

        internal UserFacade()
        {
            _emails = new();
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                Log.Error("Email cannot be empty.");
                throw new ArgumentNullException("Email cannot be empty.");
            }
            if (_emails.ContainsKey(email))
            {
                Log.Error(email + " already exists.");
                throw new InvalidOperationException(email + " already exists.");
            }
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Log.Error("Invalid email format.");
                throw new FormatException("Invalid email format.");
            }
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6 || password.Length > 20)
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
                Log.Error(email + " doesn't exist.");
                throw new KeyNotFoundException(email + " doesn't exist.");
            }
        }

        internal UserBL Login(string email, string password)
        {
            AuthenticateUser(email);
            return _emails[email].Login(password);
        }

        internal void Logout(string email)
        {
            AuthenticateUser(email);
            _emails[email].Logout();
        }

        internal UserBL Register(string email, string password)
        {
            ValidateEmail(email);
            ValidatePassword(password);
            UserBL newUser = new(email, password);
            _emails[email] = newUser;
            Log.Info($"New user registered: {email}");
            return newUser;
        }

        internal UserBL GetUser(string email) => 
            _emails[email];

        internal void LoadData()
        {
            foreach (UserDTO dto in new UserDTO().SelectAll())
                _emails[dto.Email] = new(dto);
            Log.Info("All users loaded from persistent storage.");
        }
    }
}