using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class UserDTO : IDTO
    {
        internal const string USER_EMAIL_COLUMN_NAME = "email";
        internal const string USER_PASSWORD_COLUMN_NAME = "password";
        internal const string USER_LOGGED_IN_COLUMN_NAME = "logged_in";
        private string _email;
        private string _password;
        private bool _loggedIn;
        private readonly UserController _controller;

        internal string Email 
        { 
            get => _email; 
            set { _controller.Update(USER_EMAIL_COLUMN_NAME, _email, USER_EMAIL_COLUMN_NAME, value); _email = value; } 
        }

        internal string Password 
        { 
            get => _password; 
            set { _controller.Update(USER_EMAIL_COLUMN_NAME, _email, USER_PASSWORD_COLUMN_NAME, value); _password = value; } 
        }

        internal bool LoggedIn
        {
            get => _loggedIn;
            set { _controller.Update(USER_EMAIL_COLUMN_NAME, _email, USER_LOGGED_IN_COLUMN_NAME, value); _loggedIn = value; }
        }

        internal UserDTO(string email, string password, bool loggedIn)
        {
            _email = email;
            _password = password;
            _loggedIn = loggedIn;
            _controller = new UserController();
        }

        internal UserDTO(string email)
        {
            _email = email;
            _controller = new UserController();
        }

        internal UserDTO()
        {
            _controller = new UserController();
        }

        internal void Insert()
        {
            _controller.Insert(this);
        }

        internal void Delete()
        {
            _controller.Delete(USER_EMAIL_COLUMN_NAME, _email);
        }

        internal List<UserDTO> SelectAll()
        {
            return _controller.SelectAll();
        }

        public string[] GetColumnNames() => new[] { USER_EMAIL_COLUMN_NAME, USER_PASSWORD_COLUMN_NAME, USER_LOGGED_IN_COLUMN_NAME };
        public object[] GetColumnValues() => new object[] { _email, _password, _loggedIn };
    }
}
