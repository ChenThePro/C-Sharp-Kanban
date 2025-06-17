using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Collections.Generic;
using System.Net.Mail;
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
            _emails = new(StringComparer.OrdinalIgnoreCase);
        }

        internal void AuthenticateUser(string email)
        {
            AuthenticateString(email, "Email");
            if (!_emails.ContainsKey(email))
            {
                Log.Error(email + " doesn't exist in the system.");
                throw new KeyNotFoundException(email + " doesn't exist in the system.");
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

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ValidateEmail(string email)
        {
            AuthenticateString(email, "Email");
            if (_emails.ContainsKey(email))
            {
                Log.Error(email + " already exists.");
                throw new InvalidOperationException(email + " already exists.");
            }
            if (!IsValidEmail(email))
            {
                Log.Error("Invalid email format.");
                throw new FormatException("Invalid email format.");
            }
        }

        private void ValidatePassword(string password)
        {
            AuthenticateString(password, "Password");
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

        private void AuthenticateString(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Log.Error($"{name} cannot be null or empty.");
                throw new ArgumentNullException($"{name} cannot be null or empty.");
            }
        }

        internal void ChangePassword(string email, string newPassword)
        {
            AuthenticateUser(email);
            ValidatePassword(newPassword);
            UserBL user = _emails[email];
            user.Password = newPassword;
        }
    }
}