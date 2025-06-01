using IntroSE.Kanban.Backend.BuisnessLayer.UserPackage;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class BoardFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const int DESC_MAX = 300, TITLE_MAX = 50, MAX_COLUMN_INDEX = 2, 
            MAX_IN_PROGRESS_COLUMN = 1;

        private readonly UserFacade _userfacade;
        private readonly Dictionary<int, BoardBL> _boards;

        internal BoardFacade(UserFacade userfacade)
        {
            _userfacade = userfacade;
            _boards = new();
        }

        internal BoardBL CreateBoard(string email, string boardName)
        {
            Log.Info($"Attempting to create board '{boardName}' for user '{email}'.");
            AuthenticateUser(email);
            AuthenticateString(boardName, "Board name");
            UserBL user = _userfacade.GetUser(email);
            user.BoardExists(boardName);
            BoardBL board = new(email, boardName);
            user.CreateBoard(board);
            _boards.Add(board.Id, board);
            Log.Info($"Board '{boardName}' successfully created for user '{email}'.");
            return board;
        }

        internal void DeleteBoard(string email, string boardName)
        {
            Log.Info($"User '{email}' is deleting board '{boardName}'.");
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.Delete(email);
            foreach (string member in board.Members)
                _userfacade.GetUser(member).DeleteBoard(boardName);
            _boards.Remove(board.Id);
            Log.Info($"Board '{boardName}' deleted for user '{email}'.");
        }

        internal TaskBL AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            Log.Info($"Adding task to board '{boardName}' by '{email}'.");
            AuthenticateString(title, "Title");
            AuthenticateTitleLength(title);
            AuthenticateDescription(description);
            DateTime createdAt = DateTime.Today;
            AuthenticateInteger(dueDate.CompareTo(createdAt), "Due date", true);
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            return board.AddTask(email, title, description, dueDate, createdAt);
        }

        internal void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            Log.Info($"User '{email}' is setting column limit to {limit} in column {columnOrdinal} of board '{boardName}'.");
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, MAX_COLUMN_INDEX);
            if (limit != -1)
            {
                if (limit == 0)
                {
                    Log.Error("Limit cannot be zero.");
                    throw new ArgumentOutOfRangeException("Limit cannot be zero.");
                }
                AuthenticateInteger(limit, "Limit");
            }
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.LimitColumn(email, columnOrdinal, limit);
        }

        internal void AdvanceTask(string email, string boardName, int columnOrdinal, int taskID)
        {
            Log.Info($"User '{email}' is advancing task {taskID} in board '{boardName}'.");
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, MAX_IN_PROGRESS_COLUMN);
            AuthenticateInteger(taskID, "Task ID");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.AdvanceTask(email, columnOrdinal, taskID);
        }

        internal void UpdateTask(string email, string boardName, int columnOrdinal, int taskID, DateTime? dueDate, string title, string description)
        {
            Log.Info($"User '{email}' is updating task {taskID} in board '{boardName}'.");
            if (title == null && description == null && dueDate == null)
            {
                Log.Error("At least one field (title, description, or due date) must be provided.");
                throw new ArgumentNullException("At least one field (title, description, or due date) must be provided.");
            }
            if (title != null)
            {
                if (title.Trim().Length == 0)
                {
                    Log.Error("Title cannot be empty.");
                    throw new ArgumentException("Title cannot be empty.");
                }
                AuthenticateTitleLength(title);
            }
            AuthenticateDescription(description);
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, MAX_IN_PROGRESS_COLUMN);
            AuthenticateInteger(taskID, "Task ID");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.UpdateTask(email, columnOrdinal, taskID, dueDate, title, description);
        }

        internal List<TaskBL> GetColumn(string email, string boardName, int columnOrdinal)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, MAX_COLUMN_INDEX);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            return board.GetColumn(columnOrdinal);
        }

        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, MAX_COLUMN_INDEX);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            return board.GetColumnLimit(columnOrdinal);
        }

        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, MAX_COLUMN_INDEX);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            return board.GetColumnName(columnOrdinal);
        }

        internal List<TaskBL> InProgressTasks(string email)
        {
            AuthenticateUser(email);
            return _userfacade.GetUser(email).InProgressTasks();
        }

        internal List<int> GetUserBoards(string email)
        {
            AuthenticateUser(email);
            return _userfacade.GetUser(email).GetUserBoards();
        }

        internal void JoinBoard(string email, int boardID)
        {
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = GetBoardById(boardID);
            board.JoinBoard(email);
            user.CreateBoard(board);
        }

        internal void LeaveBoard(string email, int boardID)
        {
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = GetBoardById(boardID);
            board.LeaveBoard(email);
            user.LeaveBoard(board);
        }

        internal string GetBoardName(int boardID) => GetBoardById(boardID).Name;

        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            AuthenticateUser(currentOwnerEmail);
            _userfacade.AuthenticateUser(newOwnerEmail);
            UserBL user = _userfacade.GetUser(currentOwnerEmail);
            BoardBL board = user.GetBoard(boardName);
            board.TransferOwnership(currentOwnerEmail, newOwnerEmail);
        }

        internal void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, MAX_IN_PROGRESS_COLUMN);
            AuthenticateInteger(taskID, "Task ID");
            _userfacade.AuthenticateUser(emailAssignee);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.AssignTask(email, columnOrdinal, taskID, emailAssignee);
        }

        internal void LoadData()
        {
            Log.Info("Loading board data from storage.");
            _userfacade.LoadData();
            foreach (BoardDTO boardDTO in new BoardDTO().SelectAll())
                _boards[boardDTO.Id] = new(boardDTO);
        }

        internal void DeleteData()
        {
            Log.Warn("Deleting all data and resetting auto-increments.");
            new TaskController().DeleteAllAndResetAutoIncrement();
            new BoardUserController().DeleteAll();
            new BoardController().DeleteAllAndResetAutoIncrement();
            new UserController().DeleteAll();
        }

        private void AuthenticateString(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Log.Error($"{name} cannot be null or empty.");
                throw new ArgumentNullException($"{name} cannot be null or empty.");
            }
        }

        private void AuthenticateTitleLength(string title)
        {
            if (title.Length > TITLE_MAX)
            {
                Log.Error($"Title must not exceed {TITLE_MAX} characters.");
                throw new ArgumentOutOfRangeException($"Title must not exceed {TITLE_MAX} characters.");
            }
        }

        private void AuthenticateDescription(string description)
        {
            if (description != null && (description.Length > DESC_MAX || description.Trim().Length == 0))
            {
                Log.Error($"Description must be non-empty and at most {DESC_MAX} characters.");
                throw new ArgumentOutOfRangeException($"Description must be non-empty and at most {DESC_MAX} characters.");
            }
        }

        private void AuthenticateUser(string email)
        {
            if (!_userfacade._emails.TryGetValue(email, out var user))
            {
                Log.Error(email + " doesn't exist.");
                throw new KeyNotFoundException(email + " doesn't exist.");
            }
            if (!user.LoggedIn)
            {
                Log.Error(email + " is not logged in.");
                throw new InvalidOperationException(email + " is not logged in.");
            }
        }

        private void AuthenticateColumn(int ordinal, int max)
        {
            if (ordinal < 0 || ordinal > max)
            {
                Log.Error($"Column index must be between 0 and {max}.");
                throw new ArgumentOutOfRangeException($"Column index must be between 0 and {max}.");
            }
        }

        private void AuthenticateInteger(int num, string name, bool isDueComparison = false)
        {
            if (num < 0)
            {
                if (isDueComparison)
                {
                    Log.Error("Due date cannot be earlier than the creation date.");
                    throw new ArgumentOutOfRangeException("Due date cannot be earlier than the creation date.");
                }
                Log.Error($"{name} cannot be negative.");
                throw new ArgumentOutOfRangeException($"{name} cannot be negative.");
            }
        }

        private BoardBL GetBoardById(int boardID)
        {
            if (!_boards.TryGetValue(boardID, out var board))
            {
                Log.Error(boardID + " Board not found.");
                throw new KeyNotFoundException(boardID + " Board not found.");
            }
            return board;
        }
    }
}