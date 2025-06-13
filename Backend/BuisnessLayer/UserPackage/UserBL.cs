using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.UserPackage
{
    internal class UserBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string _email, _password;
        private bool _loggedIn;

        internal string Email { 
            get => _email; 
            private set { _email = value; _userDTO.Email = value; } 
        }

        internal string Password { 
            get => _password; 
            private set { _password = value; _userDTO.Password = value; } 
        }

        internal bool LoggedIn { 
            get => _loggedIn; 
            private set { _loggedIn = value; _userDTO.LoggedIn = value; } 
        }

        internal List<BoardBL> Boards { get; init; }

        private readonly UserDTO _userDTO;

        internal UserBL(string email, string password)
        {
            _email = email;
            _password = password;
            _loggedIn = true;
            Boards = new();
            _userDTO = new(_email, _password, _loggedIn);
            _userDTO.Insert();
            Log.Info($"User created and logged in: {_email}");
        }

        internal UserBL(UserDTO userDTO)
        {
            _email = userDTO.Email;
            _password = userDTO.Password;
            _loggedIn = userDTO.LoggedIn;
            _userDTO = userDTO;
            Boards = new();
            List<int> boardIds = new BoardUserDTO(Email).GetBoards();
            List<BoardDTO> allBoards = new BoardDTO().SelectAll();
            foreach (BoardDTO boardDTO in allBoards.Where(board => boardIds.Contains(board.Id)))
                Boards.Add(new BoardBL(boardDTO));
            Log.Info($"User loaded from database: {_email}");
        }

        internal UserBL Login(string password)
        {
            if (_password != password)
            {
                Log.Error("Incorrect password.");
                throw new UnauthorizedAccessException("Password incorrect.");
            }
            if (_loggedIn)
            {
                Log.Error("User already logged in.");
                throw new InvalidOperationException("User is already logged in.");
            }
            LoggedIn = true;
            Log.Info($"User logged in: {_email}");
            return this;
        }

        internal void Logout()
        {
            if (!_loggedIn)
            {
                Log.Error("Attempted logout while not logged in.");
                throw new InvalidOperationException("Attempted logout while not logged in.");
            }
            LoggedIn = false;
            Log.Info($"User logged out: {_email}");
        }

        internal BoardBL GetBoard(string boardName)
        {
            BoardBL board = Boards.FirstOrDefault(b => b.Name.Equals(boardName, StringComparison.OrdinalIgnoreCase));
            if (board == null)
            {
                Log.Error($"Board '{boardName}' not found for user '{_email}'.");
                throw new KeyNotFoundException($"Board '{boardName}' not found for user '{_email}'.");
            }
            return board;
        }

        internal void CheckCreateBoard(string boardName)
        {
            bool exists = Boards.Any(b => b.Name.Equals(boardName, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                Log.Error($"Board '{boardName}' already exists for user '{_email}'.");
                throw new InvalidOperationException($"Board '{boardName}' already exists for user '{_email}'.");
            }
        }

        internal void CreateBoard(BoardBL board)
        {
            Boards.Add(board);
            Log.Info($"Board '{board.Name}' added to user '{_email}'.");
        }

        internal void DeleteBoard(string boardName)
        {
            BoardBL board = GetBoard(boardName);
            RemoveBoard(board);
        }

        internal List<TaskBL> InProgressTasks() =>
            Boards.SelectMany(board => board.GetColumnTasks(1)).Where(task => task.Assignee == _email).ToList();

        internal List<int> GetBoardsAsId() =>
            Boards.Select(board => board.Id).ToList();

        internal void LeaveBoard(BoardBL board) => RemoveBoard(board);

        private void RemoveBoard(BoardBL board)
        {
            Boards.Remove(board);
            Log.Info($"Board '{board.Name}' removed from user '{_email}'.");
        }
    }
}