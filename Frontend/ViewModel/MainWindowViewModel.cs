using Frontend.Controllers;
using Frontend.Model;
using Frontend.Utils;
using IntroSE.Kanban.Backend.ServiceLayer;

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
        private string _generatedCode;
        private string _newPassword;
       
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                RaisePropertyChanged(nameof(NewPassword));
            }
        }


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

        public bool UpdatePassword(string email, string newPassword)
        {
            try
            {
                _controller.changePassword(email, newPassword);
                Password = newPassword;
            }
            catch (Exception ex) {
                ErrorMessage = $"Error changing password: {ex.Message}";
                return false;
            }
            return true;
        }


        public bool SendResetCodeToEmail(string email)
        {
            try
            {
                _controller.AuthenticateUser(email); // Optional validation

                _generatedCode = new Random().Next(100000, 999999).ToString(); // 6-digit code

                TempData.Save(email, _generatedCode);

                return new EmailService().SendResetCode(email, _generatedCode);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error sending reset code: {ex.Message}";
                return false;
            }
        }
    }
}