using System.Windows;
using System.Windows.Controls;
using Frontend.Model;
using Frontend.Utils;
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) =>
            _viewModel.Password = ((PasswordBox)sender).Password;

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e) =>
            _viewModel.ConfirmPassword = ((PasswordBox)sender).Password;

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.ValidatePasswords())
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                UserModel? user = _viewModel.SignUp();
                if (user != null)
                {
                    MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.Properties["CurrentUser"] = user;
                    UserHomeWindow userHome = new();
                    Application.Current.MainWindow = userHome;
                    Close();
                    userHome.Show();
                }
                else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            UserModel? user = _viewModel.SignIn();
            if (user != null)
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Properties["CurrentUser"] = user;
                UserHomeWindow userHome = new();
                Application.Current.MainWindow = userHome;
                Close();
                userHome.Show();
            }
            else
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
                ForgotPasswordButton.Visibility = Visibility.Visible;
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ResetPasswordWindow resetWindow = new(new InMemoryTempCodeService());
            resetWindow.ShowDialog();
        }
    }
}