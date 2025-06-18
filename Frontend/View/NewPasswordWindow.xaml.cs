using System.Windows;
using System.Windows.Controls;
using Frontend.ViewModel;

namespace Frontend.View
{
    public partial class NewPasswordWindow : Window
    {
        private readonly NewPasswordWindowViewModel _viewModel;

        public NewPasswordWindow(string email)
        {
            InitializeComponent();
            _viewModel = new(email);
            DataContext = _viewModel;
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e) =>
            _viewModel.NewPassword = ((PasswordBox)sender).Password;

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.ResetPassword())
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}