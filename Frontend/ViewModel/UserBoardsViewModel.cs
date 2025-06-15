// This code will implement board creation, deletion, join, and leave functionality in the ViewModel layer.
using Frontend.Command;
using Frontend.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Frontend.ViewModel
{
    public class UserBoardsViewModel : NotifiableObject
    {
        private readonly UserModel _user;
        private readonly BackendController _controller;

        public ObservableCollection<BoardModel> Boards => _user.Boards;

        private string _newBoardName;
        public string NewBoardName { get => _newBoardName; set { _newBoardName = value; RaisePropertyChanged(); } }

        private string _joinBoardOwner;
        private string _joinBoardName;
        public string JoinBoardOwner { get => _joinBoardOwner; set { _joinBoardOwner = value; RaisePropertyChanged(); } }
        public string JoinBoardName { get => _joinBoardName; set { _joinBoardName = value; RaisePropertyChanged(); } }

        public ICommand CreateBoardCommand { get; }
        public ICommand DeleteBoardCommand { get; }
        public ICommand JoinBoardCommand { get; }
        public ICommand LeaveBoardCommand { get; }

        public UserBoardsViewModel(UserModel user)
        {
            _user = user;
            _controller = user.Controller;

            CreateBoardCommand = new RelayCommand(_ => CreateBoard());
            DeleteBoardCommand = new RelayCommand(DeleteBoard);

            _newBoardName = string.Empty;
            _joinBoardOwner = string.Empty;
            _joinBoardName = string.Empty;
        }

        private void CreateBoard()
        {
            try
            {
                var boardModel = _controller.CreateBoard(_user.Email, NewBoardName);
                Boards.Add(boardModel);
                NewBoardName = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create board: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBoard(object? param)
        {
            if (param is BoardModel board)
            {
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
        }
    }
}
