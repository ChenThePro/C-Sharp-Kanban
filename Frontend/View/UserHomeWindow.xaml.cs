using Frontend.Model;
using Frontend.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for UserHomeWindow.xaml
    /// </summary>
    public partial class UserHomeWindow : Window
    {
        private readonly UserHomeWindowViewModel _viewModel;
        private readonly BoardController Bcontroller;


        public UserHomeWindow(UserModel user)
        {
            InitializeComponent();
            Bcontroller = new BoardController(user.Controller.GetFactory());
            _viewModel = new(user , Bcontroller)
            {
                CloseAction = () => Close()
            };
            DataContext = _viewModel;
        }
        private void ToggleBoardExpand(object sender, MouseButtonEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is BoardModel board)
                board.IsExpanded = !board.IsExpanded;
        }

        private void ToggleColumnExpand(object sender, MouseButtonEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is ColumnModel column)
                column.IsExpanded = !column.IsExpanded;
        }
    }
}
