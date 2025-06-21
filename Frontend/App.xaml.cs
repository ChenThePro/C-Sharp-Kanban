using Frontend.Model;
using System.Windows;

namespace Frontend
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            if (Current.Properties.Contains("CurrentUser"))
                if (Current.Properties["CurrentUser"] is UserModel user)
                    user.Controller.Logout(user.Email);
            base.OnExit(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SwitchTheme(false);
        }

        public static void SwitchTheme(bool isDark)
        {
            var dicts = Current.Resources.MergedDictionaries;
            var light = dicts.FirstOrDefault(d => d.Source?.OriginalString.Contains("Themes/Light.xaml") == true);
            var dark = dicts.FirstOrDefault(d => d.Source?.OriginalString.Contains("Themes/Dark.xaml") == true);
            if (light != null) dicts.Remove(light);
            if (dark != null) dicts.Remove(dark);
            ResourceDictionary newDict = new()
            {
                Source = new(isDark ? "Themes/Dark.xaml" : "Themes/Light.xaml", UriKind.Relative)
            };
            dicts.Add(newDict);
        }
    }
}