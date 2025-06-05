using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class UserDTO : IDTO
    {
        private const string EMAIL = "email", PASSWORD = "password", 
            LOGGED_IN = "logged_in";

        private string _email, _password;
        private bool _loggedIn;

        internal string Email
        {
            get => _email;
            set { Update(EMAIL, value); _email = value; }
        }

        internal string Password
        {
            get => _password;
            set { Update(PASSWORD, value); _password = value; }
        }

        internal bool LoggedIn
        {
            get => _loggedIn;
            set { Update(LOGGED_IN, value); _loggedIn = value; }
        }

        private readonly UserController _controller;

        internal UserDTO(string email, string password, bool loggedIn)
        {
            _email = email;
            _password = password;
            _loggedIn = loggedIn;
            _controller = new();
        }

        internal UserDTO() => _controller = new();

        internal void Insert() => _controller.Insert(this);

        internal List<UserDTO> SelectAll() => _controller.SelectAll();

        public void Update(string column, object newValue) =>
            _controller.Update(EMAIL, _email, column, newValue);

        public string[] GetColumnNames() => new[] { EMAIL, PASSWORD, LOGGED_IN };

        public object[] GetColumnValues() => new object[] { _email, _password, _loggedIn };
    }
}