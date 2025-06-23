using Frontend.Utils;
using Frontend.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using MahApps.Metro.IconPacks;

namespace Frontend.View
{
    public partial class NewPasswordWindow : Window
    {
        private readonly NewPasswordWindowViewModel _viewModel;
        private bool _isNewPasswordVisible = false;

        public NewPasswordWindow(string email)
        {
            InitializeComponent();
            _viewModel = new(email);
            DataContext = _viewModel;
            TogglePasswordVisibility(NewPasswordGrid, _isNewPasswordVisible, _viewModel.NewPassword, "New Password", NewPasswordBox_PasswordChanged, ToggleNewPasswordVisibility);
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e) =>
            _viewModel.NewPassword = ((PasswordBox)sender).Password;

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.ResetPassword())
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ToggleNewPasswordVisibility(object sender, RoutedEventArgs e)
        {
            _isNewPasswordVisible = !_isNewPasswordVisible;
            TogglePasswordVisibility(NewPasswordGrid, _isNewPasswordVisible, _viewModel.NewPassword, "New Password", NewPasswordBox_PasswordChanged, ToggleNewPasswordVisibility);
        }

        private void TogglePasswordVisibility(Grid grid, bool isVisible, string currentValue, string hintText, RoutedEventHandler changedHandler, RoutedEventHandler toggleHandler)
        {
            grid.Children.Clear();
            if (isVisible)
            {
                TextBox textBox = new()
                {
                    Text = currentValue,
                    Style = (Style)FindResource("MaterialDesignFloatingHintTextBox"),
                    Foreground = (Brush)TryFindResource("ForegroundBrush"),
                    CaretBrush = (Brush)TryFindResource("PrimaryBrush"),
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                HintAssist.SetHint(textBox, hintText);
                HintAssist.SetIsFloating(textBox, true);
                textBox.TextChanged += (s, e) =>
                {
                    _viewModel.NewPassword = textBox.Text;
                };
                grid.Children.Add(textBox);
                AddEyeToggle(grid, toggleHandler, true);
            }
            else
            {
                PasswordBox passwordBox = new()
                {
                    Password = currentValue,
                    Style = (Style)FindResource("MaterialDesignFloatingHintPasswordBox"),
                    Foreground = (Brush)TryFindResource("ForegroundBrush"),
                    CaretBrush = (Brush)TryFindResource("PrimaryBrush"),
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                HintAssist.SetHint(passwordBox, hintText);
                HintAssist.SetIsFloating(passwordBox, true);
                passwordBox.PasswordChanged += changedHandler;
                grid.Children.Add(passwordBox);
                AddEyeToggle(grid, toggleHandler, false);
            }
        }

        private void AddEyeToggle(Grid parent, RoutedEventHandler toggleHandler, bool isVisible)
        {
            ToggleButton toggleButton = new()
            {
                Width = 30,
                Height = 30,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new(0, 0, 10, 0),
                IsChecked = isVisible,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new(0),
                Cursor = Cursors.Hand,
                Style = (Style)FindResource("MaterialDesignToolButton")
            };
            RippleAssist.SetFeedback(toggleButton, Brushes.Transparent);

            PackIconMaterial icon = new()
            {
                Kind = isVisible ? PackIconMaterialKind.EyeOff : PackIconMaterialKind.Eye,
                Width = 20,
                Height = 20,
                Foreground = (Brush)Application.Current.Resources["PrimaryBrush"]
            };
            toggleButton.Content = icon;
            toggleButton.Click += toggleHandler;
            parent.Children.Add(toggleButton);
        }
    }
}