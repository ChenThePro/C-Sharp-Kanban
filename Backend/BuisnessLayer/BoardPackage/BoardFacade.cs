using IntroSE.Kanban.Backend.BuisnessLayer.UserPackage;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class BoardFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const int DESC_MAX = 300;
        private const int TITLE_MAX = 50;
        private readonly UserFacade _userfacade;
        private readonly Dictionary<int,BoardBL> _boardNameByID = new Dictionary<int ,BoardBL>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardFacade"/> class.
        /// </summary>
        internal BoardFacade(UserFacade userfacade)
        {
            _userfacade = userfacade;
        }

        private void AuthenticateString(string name, string type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.Error($"{type} cannot be empty.");
                throw new ArgumentNullException($"{type} cannot be empty.");
            }
        }

        private void AuthenticateTitleLength(string title)
        {
            if (title.Length > TITLE_MAX)
            {
                Log.Error("Title cannot exceed 50 characters.");
                throw new ArgumentOutOfRangeException("Title cannot exceed 50 characters.");
            }
        }

        private void AuthenticateDescription(string description)
        {
            if (description != null && (description.Length > DESC_MAX || description.Trim().Length == 0))
            {
                Log.Error("Description must be non-empty and at most 300 characters.");
                throw new ArgumentOutOfRangeException("Description must be non-empty and at most 300 characters.");
            }
        }

        private void AuthenticateUser(string email)
        {
            if (!_userfacade._emails.ContainsKey(email))
            {
                Log.Error("User doesn't exist.");
                throw new KeyNotFoundException("User doesn't exist.");
            }
            if (!_userfacade._emails[email].LoggedIn)
            {
                Log.Error("User is not logged in.");
                throw new InvalidOperationException("User is not logged in.");
            }
        }

        private void AuthenticateColumn(int columnOrdinal, int limit)
        {
            if (columnOrdinal > limit || columnOrdinal < 0)
            {
                Log.Error("Column index is out of valid range.");
                throw new ArgumentOutOfRangeException("Column index is out of valid range.");
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

        /// <summary>
        /// The AddTask function adds a task to a board after validating input, 
        /// checking if the board exists and ensuring the task name isn't already taken. 
        /// If valid, it creates a new TaskSL object and adds it to the Backlog
        /// </summary>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="due"></param>
        /// <param name="taskID"></param>
        /// <param name="creatinTime"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException">Thrown when email does not exist or not logged in.</exception>
        internal TaskBL AddTask(string email, string boardName, string title, string description, DateTime dueDate, int taskID)
        {
            AuthenticateString(title, "Title");
            AuthenticateTitleLength(title);
            AuthenticateDescription(description);
            AuthenticateInteger(taskID, "Id");
            DateTime created_at = DateTime.Today;
            AuthenticateInteger(dueDate.CompareTo(created_at), "", true);
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            if (board.GetTaskByIdAndColumn(0, taskID) != null || board.GetTaskByIdAndColumn(1, taskID) != null || board.GetTaskByIdAndColumn(2, taskID) != null)
            {
                Log.Error("Task ID is already used in this board.");
                throw new InvalidOperationException("Task ID is already used in this board.");
            }
            return board.AddTask(title, description, dueDate, created_at, taskID, 0);
        }

        /// <summary>
        /// Creates a new board for a user.
        /// </summary>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="email">The user's email.</param>
        /// <returns>The created <see cref="BoardBL"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if boardName or email is null or empty.</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException">Thrown if a board with the given name already exists.</exception>
        internal BoardBL CreateBoard(string email, string boardName, int BoardID)
        {
            AuthenticateUser(email);
            AuthenticateString(boardName, "Board name");
            AuthenticateInteger(BoardID, "Id");
            UserBL user = _userfacade.GetUser(email);
            if (user.BoardExists(boardName))
            {
                Log.Error("A board with the given name already exists.");
                throw new InvalidOperationException("A board with the given name already exists.");
            }
            BoardBL board = new BoardBL(email, boardName, BoardID);
            user.CreateBoard(board);
            _boardNameByID.Add(board.Id, board);
            Log.Info($"New board '{boardName}' created for {email}.");
            return board;
        }

        /// <summary>
        /// Deletes an existing board.
        /// </summary>
        /// <param name="boardName">The name of the board to delete.</param>
        /// <param name="email">The user's email.</param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        internal void DeleteBoard(string email, string boardName)
        {
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            user.DeleteBoard(boardName);
            _boardNameByID.Remove(board.Id);
            Log.Info($"Board '{boardName}' deleted for {email}.");
        }

        /// <summary>
        /// Sets a task limit for a specific column in a board.
        /// </summary>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="column">The column index.</param>
        /// <param name="limit">The new task limit (must be non-negative).</param>
        /// <param name="email">The user's email.</param>
        /// <exception cref="InvalidOperationException">Thrown if the column index is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the board does not exist.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, 2);
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

        /// <summary>
        /// Moves a task to the next column.
        /// </summary>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="column">The current column index.</param>
        /// <param name="taskID">The task ID.</param>
        /// <param name="email">The user's email.</param>
        /// <exception cref="InvalidOperationException">Thrown if the column index is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the board or task does not exist.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal void AdvanceTask(string email, string boardName, int columnOrdinal, int taskID)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, 1);
            AuthenticateInteger(taskID, "Id");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.AdvanceTask(email, columnOrdinal, taskID);
        }

        /// <summary>
        /// Updates a task's details.
        /// </summary>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="title">The new title.</param>
        /// <param name="due">The new due date.</param>
        /// <param name="description">The new description.</param>
        /// <param name="id">The task ID.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="column">The column index.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the board or task does not exist.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException">Thrown if the user doesn't exist or is not logged in ot task id is taken or invalid column.</exception>
        internal void UpdateTask(string email, string boardName, int columnOrdinal, int taskID, DateTime? dueDate, string title, string description)
        {
            if (title == null && description == null && dueDate == null)
            {
                Log.Error("At least one field (title, description, or due date) must be provided for update.");
                throw new ArgumentNullException("At least one field (title, description, or due date) must be provided for update.");
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
            AuthenticateColumn(columnOrdinal, 2);
            AuthenticateInteger(taskID, "Id");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.UpdateTask(email, columnOrdinal, taskID, dueDate, title, description);
        }

        /// <summary>
        /// Retrieves all tasks in a specified column.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="boardName">The board's name.</param>
        /// <param name="columnOrdinal">The column index.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns>A list of <see cref="TaskBL"/>.</returns>
        internal List<TaskBL> GetColumn(string email, string boardName, int columnOrdinal)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, 2);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            return board.GetColumn(columnOrdinal);
        }

        /// <summary>
        /// Retrieves the task limit for a specified column.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="boardName">The board's name.</param>
        /// <param name="columnOrdinal">The column index.</param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>The task limit.</returns>
        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, 2);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            return board.GetColumnLimit(columnOrdinal);
        }

        /// <summary>
        /// Gets the name of a specific column in a board.
        /// </summary>
        /// <param name="email">The email of the requesting user.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column.</param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>The name of the column.</returns>
        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, 2);
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            return board.GetColumnName(columnOrdinal);
        }

        /// <summary>
        /// Retrieves all in-progress tasks across all boards.
        /// </summary>
        /// <param name="email">The email of the requesting user.</param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns>A list of in-progress <see cref="TaskSL"/> objects.</returns>
        internal List<TaskBL> InProgressTasks(string email)
        {
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            return user.InProgressTasks();
        }
      
        internal string GetUserBoards(string email)
        {
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            return user.GetUserBoards(email);
        }
      
        internal string JoinBoard(string email, int boardID)
        {
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            return user.JoinBoard(email, boardID);
        }
      
        internal string LeaveBoard(string email, int boardID)
        {
            AuthenticateUser(email);
            UserBL user = _userfacade.GetUser(email);
            return user.LeaveBoard(email, boardID);
        }
      
        internal string GetBoardName(int boardID)
        {
            _boardNameByID.TryGetValue(boardID, out var board);
            if (board != null)
            {
                return board.Name;
            }
            else
            {
                throw new KeyNotFoundException("boardId not found");
            }
        }
      
        internal string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            AuthenticateUser(currentOwnerEmail);
            AuthenticateUser(newOwnerEmail);
            UserBL user = _userfacade.GetUser(currentOwnerEmail);
            BoardBL board = user.GetBoard(boardName);
            return board.TransferOwnership(currentOwnerEmail, newOwnerEmail);
        }

        internal void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            AuthenticateUser(email);
            AuthenticateColumn(columnOrdinal, 2);
            AuthenticateInteger(taskID, "Id");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.AssignTask(email, columnOrdinal, taskID, emailAssignee);
        }
    }
}