using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Frontend.Command;
using Frontend.Model;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool ControlsEnabled => true;

        private string _email;

        public ICommand SignInCommand { get; }
        public ICommand SignUpCommand { get; }

        private readonly BackendController _controller;

        public MainWindowViewModel()
        {
            SignInCommand = new RelayCommand(_ => SignIn());
            SignUpCommand = new RelayCommand(_ => SignUp());
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            _email = string.Empty;
            _controller = ((App)Application.Current).Controller;
        }

        private void SignIn()
        {
            string json = _controller.SignIn(Email, Password);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            if (response.ErrorMessage != null)
                MessageBox.Show($"Error signing in: {response.ErrorMessage}");
            else
            {
                MessageBox.Show($"Signed in as {Email}");
                Application.Current.Properties["CurrentUserEmail"] = new UserModel(Email, Password);
            }
        }

        private void SignUp()
        {
            if (Password != ConfirmPassword)
                MessageBox.Show("Passwords do not match.");
            else
            {
                string json = _controller.SignUp(Email, Password);
                Response response = JsonSerializer.Deserialize<Response>(json)!;
                if (response.ErrorMessage != null)
                    MessageBox.Show($"Error signing in: {response.ErrorMessage}");
                else
                {
                    MessageBox.Show($"Registered as {Email}");
                    Application.Current.Properties["CurrentUserEmail"] = new UserModel(Email, Password);
                }
            }
        }
    }
}
