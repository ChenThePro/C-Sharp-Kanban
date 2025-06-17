using Frontend.Controllers;
using Frontend.Model;
using Frontend.Utils;

namespace Frontend.ViewModel
{
    public class MainWindowViewModel : NotifiableObject
    {
        private string _email, _password, _confirmPassword, _errorMessage;

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(nameof(Password)); } }
        public string ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; RaisePropertyChanged(nameof(ConfirmPassword)); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; RaisePropertyChanged(nameof(ErrorMessage)); } }


        private readonly UserController _controller;

        public MainWindowViewModel()
        {
            _email = string.Empty;
            _password = string.Empty;
            _confirmPassword = string.Empty;
            _errorMessage = string.Empty;
            _controller = ControllerFactory.Instance.UserController;
        }

        public UserModel? SignIn()
        {
            UserModel? user = null;
            try
            {
                user = _controller.SignIn(Email, Password);
            }
            catch (Exception)
            {
                ErrorMessage = "Error signing in: email or password are incorrect";
            }
            return user;
        }

        public UserModel? SignUp()
        {
            UserModel? user = null;
            try
            {
                user = _controller.SignUp(Email, Password);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error registering: {ex.Message}";
            }
            return user;
        }

        public bool ValidatePasswords()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match";
                return false;
            }
            return true;
        }
    }
}