using System.Windows.Input;
using Frontend.Command;
using Frontend.Model;
using System.Windows;
using Frontend.View;

namespace Frontend.ViewModel
{
    public class MainWindowViewModel : NotifiableObject
    {
        private string _email;
        private string _password;
        private string _confirmPassword;

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(); } }
        public string ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; RaisePropertyChanged(); } }

        private readonly BackendController _controller;

        public ICommand SignInCommand { get; }
        public ICommand SignUpCommand { get; }

        public MainWindowViewModel()
        {
            _email = string.Empty;
            _password = string.Empty;
            _confirmPassword = string.Empty;
            _controller = ((App)Application.Current).Controller;
            SignInCommand = new RelayCommand(_ => SignIn());
            SignUpCommand = new RelayCommand(_ => SignUp());
        }

        private void SignIn()
        {
            try
            {
                UserModel user = _controller.SignIn(Email, Password);
                Application.Current.Properties["CurrentUser"] = user;
                MessageBox.Show($"Signed in as {user.Email}");
                UserHomeWindow userHome = new(user);
                Application.Current.MainWindow = userHome;
                userHome.Show();
                CloseWindow();
            }
            catch (Exception)
            {
                MessageBox.Show("Error signing in: email or password are incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignUp()
        {
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                UserModel user = _controller.SignUp(Email, Password);
                Application.Current.Properties["CurrentUser"] = user;
                MessageBox.Show($"Registered as {user.Email}");
                UserHomeWindow userHome = new(user);
                Application.Current.MainWindow = userHome;
                userHome.Show();
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Action? CloseAction { get; set; }

        private void CloseWindow() => CloseAction?.Invoke();
    }
}