using System.Windows;
using System.Windows.Controls;

namespace Frontend.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModel.MainWindowViewModel vm)
            vm.Password = ((PasswordBox)sender).Password;
    }

    private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModel.MainWindowViewModel vm)
            vm.ConfirmPassword = ((PasswordBox)sender).Password;
    }
}