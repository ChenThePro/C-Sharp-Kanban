using System.Windows;
using System.Windows.Controls;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                vm.ConfirmPassword = ((PasswordBox)sender).Password;
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.ValidatePasswords())
                MessageBox.Show(_viewModel.ErrorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                UserModel? user = _viewModel.SignUp();
                if (user != null)
                {
                    Application.Current.Properties["CurrentUser"] = user;
                    MessageBox.Show($"Registered as {user.Email}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    UserHomeWindow userHome = new();
                    Application.Current.MainWindow = userHome;
                    Close();
                    userHome.Show();
                }
                else MessageBox.Show(_viewModel.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            UserModel? user = _viewModel.SignIn();
            if (user != null)
            {
                Application.Current.Properties["CurrentUser"] = user;
                MessageBox.Show($"Signed in as {user.Email}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                UserHomeWindow userHome = new();
                Application.Current.MainWindow = userHome;
                Close();
                userHome.Show();
            }
            else
            {
                MessageBox.Show(_viewModel.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ForgotPasswordButton.Visibility = Visibility.Visible;
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ResetPasswordWindow resetWindow = new();
            resetWindow.ShowDialog();
        }

    }
}