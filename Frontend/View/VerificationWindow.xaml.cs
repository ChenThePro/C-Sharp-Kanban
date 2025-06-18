using Frontend.Utils;
using Frontend.ViewModel;
using System.Windows;

namespace Frontend.View
{
    public partial class VerificationWindow : Window
    {
        private readonly VerificationWindowViewModel _viewModel;

        public VerificationWindow(string email, InMemoryTempCodeService codeService)
        {
            InitializeComponent();
            _viewModel = new(email, codeService);
            DataContext = _viewModel;
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Verify())
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                NewPasswordWindow newPasswordWindow = new(_viewModel.Email);
                Close();
                newPasswordWindow.Show();
            }
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}