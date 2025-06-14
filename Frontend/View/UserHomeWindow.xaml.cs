using Frontend.Model;
using Frontend.ViewModel;
using System.Windows;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for UserHomeWindow.xaml
    /// </summary>
    public partial class UserHomeWindow : Window
    {
        private readonly UserHomeWindowViewModel _viewModel;

        public UserHomeWindow(UserModel user)
        {
            InitializeComponent();
            _viewModel = new UserHomeWindowViewModel(user);
            DataContext = _viewModel;
        }
    }
}
