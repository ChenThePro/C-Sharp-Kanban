using Frontend.Controllers;
using Frontend.Model;
using Frontend.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Frontend.View
{
    public partial class UserHomeWindow : Window
    {
        private readonly UserHomeWindowViewModel _viewModel;

        public UserHomeWindow()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
        }

        private void ToggleBoardExpand(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement { DataContext: BoardModel board })
                board.IsExpanded = !board.IsExpanded;
        }

        private void ToggleColumnExpand(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement { DataContext: ColumnModel column })
                column.IsExpanded = !column.IsExpanded;
        }

        private void CreateBoard_Click(object sender, RoutedEventArgs e)
        {
            BoardModel? board = _viewModel.CreateBoard();
            if (board != null)
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DeleteBoard_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete this board?",
                "Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;
            if (_viewModel.DeleteBoard((sender as FrameworkElement)?.DataContext as BoardModel))
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;
            if (_viewModel.Logout())
            {
                Application.Current.Properties["CurrentUser"] = null;
                MainWindow mainWindow = new();
                Application.Current.MainWindow = mainWindow;
                Close();
                mainWindow.Show();
            }
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}