using System.Windows;
using System.Windows.Controls;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly UserController _controller;

        public MainWindow()
        {
            InitializeComponent();
            _controller = new();
            _viewModel = new(_controller)
            {
                CloseAction = Close
            };
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
    }
}