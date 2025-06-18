using System.Collections.ObjectModel;
using Frontend.Model;
using System.Windows;
using Frontend.Controllers;
using Frontend.Utils;

namespace Frontend.ViewModel
{
    public class UserHomeWindowViewModel : NotifiableObject
    {
        private string _newBoardName;

        public string NewBoardName { get => _newBoardName; set { _newBoardName = value; RaisePropertyChanged(nameof(NewBoardName)); } }
        public string Message { get; set; }
        public string Status { get; set; }
        public ObservableCollection<BoardModel> Boards => _user.Boards;

        private readonly UserModel _user;
        private readonly BoardController _controller;

        public UserHomeWindowViewModel()
        {
            _newBoardName = string.Empty;
            Message = string.Empty;
            Status = string.Empty;
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
                Message = $"Board '{board.Name}' created successfully!";
                Status = "Success";
            }   
            catch (Exception ex)
            {
                Message = $"Failed to create board: {ex.Message}";
                Status = "Error";
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
                Message = $"Board '{board.Name}' deleted successfully!";
                Status = "Success";
                return true;
            }
            catch (Exception ex)
            {
                Message = $"Failed to delete board: {ex.Message}";
                Status = "Error";
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
                Message = $"Logout failed: {ex.Message}";
                Status = "Error";
                return false;
            }
        }
    }
}