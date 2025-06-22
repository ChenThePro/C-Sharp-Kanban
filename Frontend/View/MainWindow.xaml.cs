using Frontend.Controllers;
using Frontend.Model;
using Frontend.Utils;
using Frontend.ViewModel;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Frontend.View
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        private bool _isSignInPasswordVisible = false;
        private bool _isSignUpPasswordVisible = false;
        private bool _isConfirmPasswordVisible = false;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
            TogglePasswordVisibility(SignInPasswordGrid, _isSignInPasswordVisible, _viewModel.Password, "Password", PasswordBox_PasswordChanged, ToggleSignInPasswordVisibility);
            TogglePasswordVisibility(SignUpPasswordGrid, _isSignUpPasswordVisible, _viewModel.Password, "Password", PasswordBox_PasswordChanged, ToggleSignUpPasswordVisibility);
            TogglePasswordVisibility(ConfirmPasswordGrid, _isConfirmPasswordVisible, _viewModel.ConfirmPassword, "Confirm Password", ConfirmPasswordBox_PasswordChanged, ToggleConfirmPasswordVisibility);
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            App.SwitchTheme(true);
            _viewModel.IsDarkTheme = true;
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            App.SwitchTheme(false);
            _viewModel.IsDarkTheme = false;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) =>
            _viewModel.Password = ((PasswordBox)sender).Password;

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e) =>
            _viewModel.ConfirmPassword = ((PasswordBox)sender).Password;

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.ValidatePasswords())
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                ControllerFactory.Instance.BoardController.LoadData();
                UserModel? user = _viewModel.SignUp();
                if (user != null)
                {
                    MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.Properties["CurrentUser"] = user;
                    UserHomeWindow userHome = new();
                    Application.Current.MainWindow = userHome;
                    Close();
                    userHome.Show();
                    if (user.IsDark)
                        App.SwitchTheme(true);
                    else App.SwitchTheme(false);
                }
                else MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            UserModel? user = _viewModel.SignIn();
            if (user != null)
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Properties["CurrentUser"] = user;
                UserHomeWindow userHome = new();
                Application.Current.MainWindow = userHome;
                Close();
                userHome.Show();
                if (user.IsDark)
                    App.SwitchTheme(true);
                else App.SwitchTheme(false);
            }
            else
            {
                MessageBox.Show(_viewModel.Message, _viewModel.Status, MessageBoxButton.OK, MessageBoxImage.Error);
                ForgotPasswordButton.Visibility = Visibility.Visible;
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ResetPasswordWindow resetWindow = new(new InMemoryTempCodeService());
            resetWindow.ShowDialog();
        }

        private void ToggleSignInPasswordVisibility(object sender, RoutedEventArgs e)
        {
            _isSignInPasswordVisible = !_isSignInPasswordVisible;
            TogglePasswordVisibility(SignInPasswordGrid, _isSignInPasswordVisible, _viewModel.Password, "Password", PasswordBox_PasswordChanged, ToggleSignInPasswordVisibility);
        }

        private void ToggleSignUpPasswordVisibility(object sender, RoutedEventArgs e)
        {
            _isSignUpPasswordVisible = !_isSignUpPasswordVisible;
            TogglePasswordVisibility(SignUpPasswordGrid, _isSignUpPasswordVisible, _viewModel.Password, "Password", PasswordBox_PasswordChanged, ToggleSignUpPasswordVisibility);
        }

        private void ToggleConfirmPasswordVisibility(object sender, RoutedEventArgs e)
        {
            _isConfirmPasswordVisible = !_isConfirmPasswordVisible;
            TogglePasswordVisibility(ConfirmPasswordGrid, _isConfirmPasswordVisible, _viewModel.ConfirmPassword, "Confirm Password", ConfirmPasswordBox_PasswordChanged, ToggleConfirmPasswordVisibility);
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
                    Foreground = Brushes.Black,
                    CaretBrush = Brushes.Black,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                HintAssist.SetHint(textBox, hintText);
                HintAssist.SetIsFloating(textBox, true);
                textBox.TextChanged += (s, e) =>
                {
                    if (toggleHandler == ToggleConfirmPasswordVisibility)
                        _viewModel.ConfirmPassword = textBox.Text;
                    else _viewModel.Password = textBox.Text;
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