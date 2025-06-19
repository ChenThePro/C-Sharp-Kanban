using Frontend.Utils;
using Frontend.ViewModel;
using System.Windows;

namespace Frontend.View
{
    public partial class VerificationWindow : Window
    {
        private readonly VerificationWindowViewModel _viewModel;
        private readonly InMemoryTempCodeService _codeService; 

        public VerificationWindow(string email, InMemoryTempCodeService codeService)
        {
            InitializeComponent();
            _codeService = codeService; 
            _viewModel = new(email, codeService);
            DataContext = _viewModel;
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Verify())
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                NewPasswordWindow newPasswordWindow = new(_viewModel.Email, _codeService);
                Close();
                newPasswordWindow.Show();
            }
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
