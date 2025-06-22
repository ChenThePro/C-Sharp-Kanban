using Frontend.Controllers;
using Frontend.Model;
using Frontend.Utils;

namespace Frontend.ViewModel
{
    public class MainWindowViewModel : NotifiableObject
    {
        private string _email;

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Message { get; private set; }
        public string Status { get; private set; }
        public bool IsDarkTheme { get; set; }

        private readonly UserController _controller;

        public MainWindowViewModel()
        {
            _email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            Message = string.Empty;
            Status = string.Empty;
            IsDarkTheme = false;
            _controller = ControllerFactory.Instance.UserController;
        }

        public UserModel? SignIn()
        {
            UserModel? user = null;
            try
            {
                user = _controller.SignIn(Email, Password);
                Message = $"Signed in as {Email}";
                Status = "Success";
            }
            catch (Exception)
            {
                Message = "Error signing in: email or password are incorrect";
                Status = "Error";
            }
            return user;
        }

        public UserModel? SignUp()
        {
            UserModel? user = null;
            try
            {
                user = _controller.SignUp(Email, Password);
                Message = $"Registered as {Email}";
                Status = "Success";
            }
            catch (Exception ex)
            {
                Message = $"Error registering: {ex.Message}";
                Status = "Error";
            }
            return user;
        }

        public bool ValidatePasswords()
        {
            if (Password == ConfirmPassword)
                return true;
            Message = "Passwords do not match";
            Status = "Warning";
            return false;
        }
    }
}