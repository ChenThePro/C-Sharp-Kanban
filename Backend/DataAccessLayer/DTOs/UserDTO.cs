using IntroSE.Kanban.Backend.DAL;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class UserDTO : IDTO
    {
        internal const string USER_EMAIL_COLUMN_NAME = "email";
        internal const string USER_PASSWORD_COLUMN_NAME = "password";
        private string _email;
        private string _password;
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

        internal UserDTO(string email, string password)
        {
            _email = email;
            _password = password;
            _controller = new UserController();
        }

        internal void Insert()
        {
            _controller.Insert(this);
        }

        internal void Delete()
        {
            _controller.Delete(USER_EMAIL_COLUMN_NAME, Email);
        }

        internal List<UserDTO> SelectAll()
        {
            return _controller.SelectAll();
        }

        public string[] GetColumnNames() => new[] { USER_EMAIL_COLUMN_NAME, USER_PASSWORD_COLUMN_NAME };
        public object[] GetColumnValues() => new object[] { Email, Password };
    }
}
