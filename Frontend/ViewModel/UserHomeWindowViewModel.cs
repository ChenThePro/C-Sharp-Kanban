using System.Collections.ObjectModel;
using Frontend.Command;
using Frontend.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Windows.Input;
using System.Windows;
using System.Xml.Linq;
using Frontend.View;

namespace Frontend.ViewModel
{
    internal class UserHomeWindowViewModel : NotifiableObject
    {
        private readonly UserModel _user;
        private readonly BackendController _controller;

        public ObservableCollection<BoardModel> Boards => _user.Boards;
        public string NewBoardName { get => _newBoardName; set { _newBoardName = value; RaisePropertyChanged(); } }
        private string _newBoardName = string.Empty;

        public ICommand CreateBoardCommand { get; }
        public ICommand DeleteBoardCommand { get; }
        public ICommand LogoutCommand { get; }

        public UserHomeWindowViewModel(UserModel user)
        {
            _user = user;
            _controller = user.Controller;
            LogoutCommand = new RelayCommand(_ => ExecuteLogout());
            CreateBoardCommand = new RelayCommand(_ => ExecuteCreateBoard(), _ => !string.IsNullOrWhiteSpace(NewBoardName));
            DeleteBoardCommand = new RelayCommand(b => ExecuteDeleteBoard(b as BoardModel));
        }

        private void ExecuteCreateBoard()
        {
            try
            {
                BoardModel newBoard = _controller.CreateBoard(_user.Email, NewBoardName); // Adjust as needed
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
                _controller.Logout(_user.Email);
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