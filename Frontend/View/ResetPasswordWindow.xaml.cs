using Frontend.Utils;
using Frontend.ViewModel;
using System.Windows;

namespace Frontend.View
{
    public partial class ResetPasswordWindow : Window
    {
        private readonly ResetPasswordWindowViewModel _viewModel;
        private readonly InMemoryTempCodeService _codeService;

        public ResetPasswordWindow(InMemoryTempCodeService codeService)
        {
            _codeService = codeService;
            InitializeComponent();
            _viewModel = new(_codeService);
            DataContext = _viewModel;
        }

        private void SendCode_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SendResetCode())
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                VerificationWindow verifyWindow = new(_viewModel.Email, _codeService);
                verifyWindow.Show();
                Close();
            }
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}