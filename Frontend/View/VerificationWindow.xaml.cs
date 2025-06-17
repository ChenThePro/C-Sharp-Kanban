using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for VerificationWindow.xaml
    /// </summary>
    public partial class VerificationWindow : Window
    {
        private readonly string _email;
        public VerificationWindow(string email)
        {
            InitializeComponent();
            _email = email;
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            var inputCode = CodeBox.Text;

            if (TempData.TryGetCode(_email, out string code) && inputCode == code)
            {
                TempData.Remove(_email);
                var newPasswordWindow = new NewPasswordWindow(_email);
                newPasswordWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid code.", "Error");
            }
        }
    }
}
