using IntroSE.Kanban.Backend.DAL;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class UserDTO
    {
        internal const string USER_EMAIL_COLUMN_NAME = "email";
        internal const string USER_PASSWORD_COLUMN_NAME = "password";
        private string _email;
        private string _password;
        private readonly UserController _controller;

        internal string Email 
        { 
            get => _email; 
            set { _controller.Update(_email, value, USER_EMAIL_COLUMN_NAME); _email = value; } 
        }

        internal string Password 
        { 
            get => _password; 
            set { _controller.Update(_password, value, USER_PASSWORD_COLUMN_NAME); _password = value; } 
        }

        internal UserDTO(string email, string password)
        {
            Email = email;
            Password = password;
            _controller = new UserController();
        }
    }
}
