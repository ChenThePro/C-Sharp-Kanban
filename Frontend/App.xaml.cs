using Frontend.Model;
using System.Windows;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            if (Current.Properties.Contains("CurrentUser"))
                if (Current.Properties["CurrentUser"] is UserModel user)
                    user.Controller.Logout(user.Email);
            base.OnExit(e);
        }
    }
}