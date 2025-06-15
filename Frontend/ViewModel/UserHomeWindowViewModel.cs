using Frontend.Command;
using Frontend.Model;
using Frontend.View;
using System.Windows;
using System.Windows.Input;

namespace Frontend.ViewModel
{
    internal class UserHomeWindowViewModel : NotifiableObject
    {
        private readonly UserModel _user;
        private readonly BackendController _controller;

        public ICommand LogoutCommand { get; }

        public UserHomeWindowViewModel(UserModel user)
        {
            _user = user;
            _controller = user.Controller;
            LogoutCommand = new RelayCommand(_ => ExecuteLogout());
        }

        private void ExecuteLogout()
        {
            try
            {
                _controller.Logout(_user.Email);
                Application.Current.Properties["CurrentUser"] = null;
                MainWindow mainWindow = new();
                Application.Current.MainWindow = mainWindow;
                CloseWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Logout failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Action? CloseAction { get; set; }

        private void CloseWindow() => CloseAction?.Invoke();
    }
}