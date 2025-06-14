using System.Windows.Input;
using Frontend.Command;
using Frontend.Model;
using System.Windows;

namespace Frontend.ViewModel
{
    public class MainWindowViewModel : NotifiableObject
    {
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(); } }
        public string ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; RaisePropertyChanged(); } }

        private readonly BackendController _controller;

        public ICommand SignInCommand { get; }
        public ICommand SignUpCommand { get; }

        public MainWindowViewModel()
        {
            _controller = ((App)Application.Current).Controller;
            SignInCommand = new RelayCommand(_ => SignIn());
            SignUpCommand = new RelayCommand(_ => SignUp());
        }

        private void SignIn()
        {
            try
            {
                UserModel user = _controller.SignIn(Email, Password);
                Application.Current.Properties["CurrentUserEmail"] = user;
                MessageBox.Show($"Signed in as {user.Email}");
                CloseWindow();
                new UserHomeWindowViewModel(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error signing in: {ex.Message}");
            }
        }

        private void SignUp()
        {
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }
            try
            {
                UserModel user = _controller.SignUp(Email, Password);
                Application.Current.Properties["CurrentUserEmail"] = user;
                MessageBox.Show($"Registered as {user.Email}");
                CloseWindow();
                new UserHomeWindowViewModel(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering: {ex.Message}");
            }
        }

        public Action? CloseAction { get; set; }

        private void CloseWindow() => CloseAction?.Invoke();
    }
}