﻿using Frontend.Model;
using Frontend.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Frontend.View
{
    public partial class UserHomeWindow : Window
    {
        private readonly UserHomeWindowViewModel _viewModel;

        public UserHomeWindow(bool IsDarkTheme)
        {
            InitializeComponent();
            _viewModel = new(IsDarkTheme);
            DataContext = _viewModel;
            UpdateThemeIcon();
        }

        public UserHomeWindow()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
            UpdateThemeIcon();
        }

        private void ThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.IsDarkTheme = !_viewModel.IsDarkTheme;
            App.SwitchTheme(_viewModel.IsDarkTheme);
            Controllers.ControllerFactory.Instance.UserController.ChangeTheme(((UserModel)Application.Current.Properties["CurrentUser"]!).Email);
            ((UserModel)Application.Current.Properties["CurrentUser"]!).IsDark = !(((UserModel)Application.Current.Properties["CurrentUser"]!).IsDark);
            UpdateThemeIcon();
            MessageBox.Show("Theme changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateThemeIcon()
        {
            if (_viewModel.IsDarkTheme)
            {
                MoonIcon.Visibility = Visibility.Collapsed;
                SunIcon.Visibility = Visibility.Visible;
            }
            else
            {
                MoonIcon.Visibility = Visibility.Visible;
                SunIcon.Visibility = Visibility.Collapsed;
            }
        }

        private void ToggleBoardExpand(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement { DataContext: BoardModel board })
            {
                board.IsExpanded = !board.IsExpanded;
                if (!board.IsExpanded)
                    board.Columns.ToList().ForEach(c => c.IsExpanded = false);
            }
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
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);
            if (result != MessageBoxResult.OK) return;
            if (_viewModel.DeleteBoard((sender as FrameworkElement)?.DataContext as BoardModel))
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);
            if (result != MessageBoxResult.OK) return;
            if (_viewModel.Logout())
            {
                MainWindow mainWindow = new();
                Application.Current.MainWindow = mainWindow;
                Close();
                mainWindow.Show();
                mainWindow.Theme();
                Application.Current.Properties["CurrentUser"] = null;
            }
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}