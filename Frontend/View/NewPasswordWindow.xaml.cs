using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Frontend.ViewModel;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for NewPasswordWindow.xaml
    /// </summary>
    public partial class NewPasswordWindow : Window
    {
        private readonly string _email;
        private readonly MainWindowViewModel _viewModel;
        public NewPasswordWindow(string email)
        {
            InitializeComponent();
            _email = email;
            _viewModel = new();
            DataContext = _viewModel;
        }
        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                vm.NewPassword = ((PasswordBox)sender).Password;
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var success = _viewModel.UpdatePassword(_email, _viewModel.NewPassword);
            if (success)
            {
                MessageBox.Show("Password reset successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show(_viewModel.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
