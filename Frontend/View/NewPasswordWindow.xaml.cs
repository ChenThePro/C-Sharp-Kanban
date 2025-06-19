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
        private NewPasswordWindowViewModel _viewModel;
        private bool _isNewPasswordVisible = false;

        public NewPasswordWindow()
        {
            InitializeComponent();
            _viewModel = new NewPasswordWindowViewModel(string.Empty); 
            DataContext = _viewModel;

            TogglePasswordVisibility(NewPasswordGrid, _isNewPasswordVisible, _viewModel.NewPassword, "New Password", NewPasswordBox_PasswordChanged, ToggleNewPasswordVisibility);
        }

        public NewPasswordWindow(string email, InMemoryTempCodeService codeService)
        {
            InitializeComponent();
            _viewModel = new NewPasswordWindowViewModel(email);
            DataContext = _viewModel;

            TogglePasswordVisibility(NewPasswordGrid, _isNewPasswordVisible, _viewModel.NewPassword, "New Password", NewPasswordBox_PasswordChanged, ToggleNewPasswordVisibility);
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.NewPassword = ((PasswordBox)sender).Password;
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.ResetPassword())
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                MainWindow mainWindow = new();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                var textBox = new TextBox
                {
                    Text = currentValue,
                    Style = (Style)FindResource("MaterialDesignFloatingHintTextBox"),
                    Foreground = Brushes.Black,
                    CaretBrush = Brushes.Black,
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
                var passwordBox = new PasswordBox
                {
                    Password = currentValue,
                    Style = (Style)FindResource("MaterialDesignFloatingHintPasswordBox"),
                    Foreground = Brushes.Black,
                    CaretBrush = Brushes.Black,
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
            var toggleButton = new ToggleButton
            {
                Width = 30,
                Height = 30,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 10, 0),
                IsChecked = isVisible,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                Style = (Style)FindResource("MaterialDesignToolButton")
            };

            RippleAssist.SetFeedback(toggleButton, Brushes.Transparent);

            var icon = new PackIconMaterial
            {
                Kind = isVisible ? PackIconMaterialKind.EyeOff : PackIconMaterialKind.Eye,
                Width = 20,
                Height = 20,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 184, 212))
            };

            toggleButton.Content = icon;
            toggleButton.Click += toggleHandler;
            parent.Children.Add(toggleButton);
        }
    }
}
