using System.Collections.ObjectModel;
using Frontend.Command;
using Frontend.Model;
using System.Windows.Input;
using System.Windows;
using Frontend.View;

namespace Frontend.ViewModel
{
    internal class UserHomeWindowViewModel : NotifiableObject
    {
        private readonly UserModel _user;
        private readonly BoardController _controller;
        private string _newBoardName;

        public ObservableCollection<BoardModel> Boards => _user.Boards;
        public string NewBoardName { get => _newBoardName; set { _newBoardName = value; RaisePropertyChanged(); } }
        public ICommand CreateBoardCommand { get; }
        public ICommand DeleteBoardCommand { get; }
        public ICommand LogoutCommand { get; }

        public UserHomeWindowViewModel(BoardController controller)
        {
            _user = (UserModel)Application.Current.Properties["CurrentUser"]!;
            _controller = controller;
            _newBoardName = string.Empty;
            LogoutCommand = new RelayCommand(_ => ExecuteLogout());
            CreateBoardCommand = new RelayCommand(_ => ExecuteCreateBoard());
            DeleteBoardCommand = new RelayCommand(b => ExecuteDeleteBoard(b as BoardModel));
        }

        private void ExecuteCreateBoard()
        {
            try
            {
                BoardModel newBoard = _controller.CreateBoard(_user.Email, NewBoardName);
                Boards.Add(newBoard);
                NewBoardName = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create board: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteBoard(BoardModel? board)
        {
            if (board == null) return;
            try
            {
                _controller.DeleteBoard(_user.Email, board.Name);
                Boards.Remove(board);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete board: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteLogout()
        {
            try
            {
                _user.Controller.Logout(_user.Email);
                Application.Current.Properties["CurrentUser"] = null;
                MainWindow mainWindow = new();
                Application.Current.MainWindow = mainWindow;
                CloseWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Logout failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Action? CloseAction { get; set; }

        private void CloseWindow() => CloseAction?.Invoke();
    }
}