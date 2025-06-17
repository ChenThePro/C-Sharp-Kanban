using System.Windows;
using System.Windows.Controls;
using Frontend.ViewModel;

namespace Frontend.View
{
    public partial class ResetPasswordWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public ResetPasswordWindow()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
        }

        private void EmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox box)
                _viewModel.Email = box.Text;
        }

        private void SendCode_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SendResetCodeToEmail(_viewModel.Email))
            {
                MessageBox.Show("A code has been sent to your email.", "Success");
                VerificationWindow verifyWindow = new(_viewModel.Email);
                verifyWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show(_viewModel.ErrorMessage, "Error");
            }
        }

    }
}
