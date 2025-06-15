using Frontend.Model;
using System.Windows;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public BackendController Controller { get; init; } = new();

        protected override void OnExit(ExitEventArgs e)
        {
            if (Current.Properties.Contains("CurrentUser"))
                if (Current.Properties["CurrentUser"] is UserModel user)
                    Controller.Logout(user.Email);
            base.OnExit(e);
        }
    }
}