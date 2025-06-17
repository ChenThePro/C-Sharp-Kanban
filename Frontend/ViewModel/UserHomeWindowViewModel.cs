using System.Collections.ObjectModel;
using Frontend.Model;
using System.Windows;
using Frontend.Controllers;
using Frontend.Utils;

namespace Frontend.ViewModel
{
    public class UserHomeWindowViewModel : NotifiableObject
    {
        private string _newBoardName, _errorMessage;

        public string NewBoardName { get => _newBoardName; set { _newBoardName = value; RaisePropertyChanged(nameof(NewBoardName)); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; RaisePropertyChanged(nameof(ErrorMessage)); } }
        public ObservableCollection<BoardModel> Boards => _user.Boards;

        private readonly UserModel _user;
        private readonly BoardController _controller;

        public UserHomeWindowViewModel()
        {
            _newBoardName = string.Empty;
            _errorMessage = string.Empty;
            _user = (UserModel)Application.Current.Properties["CurrentUser"]!;
            _controller = ControllerFactory.Instance.BoardController;
        }

        public BoardModel? CreateBoard()
        {
            BoardModel? board = null;
            try
            {
                board = _controller.CreateBoard(_user.Email, NewBoardName);
                Boards.Add(board);
                NewBoardName = string.Empty;
            }   
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to create board: {ex.Message}";
            }
            return board;
        }

        public bool DeleteBoard(BoardModel? board)
        {
            if (board == null) return false;
            try
            {
                board.Controller.DeleteBoard(_user.Email, board.Name);
                Boards.Remove(board);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to delete board: {ex.Message}";
                return false;
            }
        }

        public bool Logout()
        {
            try
            {
                _user.Controller.Logout(_user.Email);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Logout failed: {ex.Message}";
                return false;
            }
        }
    }
}