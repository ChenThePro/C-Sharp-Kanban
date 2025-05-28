using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.UserPackage
{
    internal class UserBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal bool LoggedIn;
        internal string Email;
        private string _password;
        private readonly List<BoardBL> _boards;
        private readonly UserDTO _userDTO;


        internal UserBL(string email, string password)
        {
            Email = email;
            LoggedIn = true;
            _password = password;
            _boards = new List<BoardBL>();
            _userDTO = new UserDTO(email, password);
            _userDTO.Insert();
        }

        public UserBL(UserDTO userDTO)
        {
            Email = userDTO.Email;
            _password = userDTO.Password;
            _boards = new List<BoardBL>();
            List<int> boardIds = new BoardUserDTO(Email).GetBoards();
            List<BoardDTO> boards = new BoardDTO().SelectAll().FindAll(board => boardIds.Contains(board.Id));
            foreach (BoardDTO boardDTO in boards)
                _boards.Add(new BoardBL(boardDTO));
            _userDTO = userDTO;
        }

        internal UserBL Login(string password)
        {
            if (_password != password)
            {
                Log.Error("Password incorrect.");
                throw new UnauthorizedAccessException("Password incorrect.");
            }
            if (LoggedIn)
            {
                Log.Error("Already logged in");
                throw new InvalidOperationException("Already logged in.");
            }
            LoggedIn = true;
            Log.Info("User logged in successfully.");
            return this;
        }

        internal void Logout()
        {
            if (!LoggedIn)
            {
                Log.Error("User not logged in.");
                throw new InvalidOperationException("User not logged in.");
            }
            LoggedIn = false;
            Log.Info("User logged out.");
        }

        internal BoardBL GetBoard(string boardName)
        {
            foreach (BoardBL board in _boards)
                if (board.Name.ToLower() == boardName.ToLower())
                    return board;
            Log.Error("Boardname doesn't exist in user.");
            throw new KeyNotFoundException("Boardname doesn't exist in user.");
        }

        internal bool BoardExists(string boardName)
        {
            foreach (BoardBL board in _boards)
                if (board.Name.ToLower() == boardName.ToLower())
                    return true;
            return false;
        }

        internal void CreateBoard(BoardBL board)
        {
            _boards.Add(board);
        }

        internal void DeleteBoard(string boardName)
        {
            _boards.Remove(GetBoard(boardName));
        }

        internal List<TaskBL> InProgressTasks()
        {
            List<TaskBL> lst = new List<TaskBL>();
            foreach (BoardBL board in _boards)
                lst.AddRange(board.GetColumn(1));
            return lst.FindAll(task => task.Assigne == Email); ;
        }
        internal List<int> GetUserBoards(string email)
        {
            List<int> boardsID = new List<int>();
            foreach(BoardBL board in _boards)
            {
                boardsID.Add(board.Id);
            }
            return boardsID;
        }
        internal void JoinBoard(BoardBL board)
        {
            if (_boards.Contains(board))
            {
                Log.Error("Board already exists in user's boards.");
                throw new InvalidOperationException("Board already exists in user's boards.");
            }
            _boards.Add(board);
        }
        internal void LeaveBoard(BoardBL board)
        {
            if (!_boards.Contains(board))
            {
                Log.Error("Board does not exist in user's boards.");
                throw new KeyNotFoundException("Board does not exist in user's boards.");
            }
            _boards.Remove(board);  
        }
    }
}