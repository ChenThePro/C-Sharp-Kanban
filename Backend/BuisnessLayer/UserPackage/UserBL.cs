using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.UserPackage
{
    internal class UserBL
    {
        internal bool LoggedIn;
        internal string Email;
        private string _password;
        private readonly List<BoardBL> _boards;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal UserBL(string email, string password)
        {
            Email = email;
            _password = password;
            LoggedIn = true;
            _boards = new List<BoardBL>();
        }

        internal UserBL Login(string password)
        {
            if (_password != password)
            {
                Log.Error("password incorrect");
                throw new UnauthorizedAccessException("password incorrect");
            }
            if (LoggedIn)
            {
                Log.Error("already logged in");
                throw new InvalidOperationException("already logged in");
            }
            LoggedIn = true;
            Log.Info("user logged in successfully");
            return this;
        }

        internal void Logout()
        {
            if (!LoggedIn)
            {
                Log.Error("user not logged in");
                throw new InvalidOperationException("user not logged in");
            }
            LoggedIn = false;
            Log.Info("user logged out");
        }

        internal BoardBL GetBoard(string boardName)
        {
            foreach (BoardBL board in _boards)
                if (board.Name.ToLower() == boardName.ToLower())
                    return board;
            Log.Error("boardname doesn't exist in user");
            throw new KeyNotFoundException("boardname doesn't exist in user");
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
            return lst;
        }
    }
}