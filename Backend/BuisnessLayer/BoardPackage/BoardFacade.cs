using IntroSE.Kanban.Backend.BuisnessLayer.UserPackage;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class BoardFacade
    {
        private readonly UserFacade _userfacade;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardFacade"/> class.
        /// </summary>
        internal BoardFacade(UserFacade userfacade)
        {
            _userfacade = userfacade;
        }

        /// <summary>
        /// helper function to check if user exist and logged in
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True if the user exists and logged in; otherwise, false.</returns>
        private bool UserExistsAndLoggedIn(string email)
        {
            return _userfacade._emails.ContainsKey(email) && _userfacade._emails[email].loggedIn;
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
        /// <param name="id"></param>
        /// <param name="creatinTime"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException">Thrown when email does not exist or not logged in.</exception>
        internal TaskBL AddTask(string boardName, string title, DateTime due, string description, DateTime creationTime, int id, string email)
        {
            if (string.IsNullOrEmpty(title) || title.Length > 50)
                throw new ArgumentException("title invalid");
            if (description != null && description.Length > 300)
                throw new InvalidOperationException("exceeds limit");
            if (id < 0)
                throw new InvalidOperationException("id can't be negative");
            if (due.CompareTo(creationTime) < 0)
                throw new InvalidOperationException("due can't be before creation");
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            if (board.GetTaskByIdAndColumn(id, 0) != null || board.GetTaskByIdAndColumn(id, 1) != null || board.GetTaskByIdAndColumn(id, 2) != null)
                throw new InvalidOperationException("task id is already taken in this board");
            return board.AddTask(title, due, description, creationTime, id, 0);
        }

        /// <summary>
        /// Creates a new board for a user.
        /// </summary>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="email">The user's email.</param>
        /// <returns>The created <see cref="BoardBL"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if boardName or email is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if a board with the given name already exists.</exception>
        internal BoardBL CreateBoard(string boardName, string email)
        {
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            if (string.IsNullOrEmpty(boardName))
                throw new ArgumentException("board name is not valid");
            UserBL user = _userfacade.GetUser(email);
            if (user.BoardExists(boardName))
                throw new InvalidOperationException("board already exists");
            BoardBL board = new BoardBL(boardName, email);
            user.CreateBoard(board);
            Log.Info("new board created - " + boardName + " for" + email);
            return board;
        }

        /// <summary>
        /// Deletes an existing board.
        /// </summary>
        /// <param name="boardName">The name of the board to delete.</param>
        /// <param name="email">The user's email.</param>
        /// <exception cref="ArgumentNullException">Thrown if boardName or email is null or empty.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the board does not exist.</exception>
        internal void DeleteBoard(string boardName, string email)
        {
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            UserBL user = _userfacade.GetUser(email);
            user.DeleteBoard(boardName);
            Log.Info(boardName + "deleted");
        }

        /// <summary>
        /// Sets a task limit for a specific column in a board.
        /// </summary>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="column">The column index.</param>
        /// <param name="limit">The new task limit (must be non-negative).</param>
        /// <param name="email">The user's email.</param>
        /// <exception cref="ArgumentException">Thrown if the limit is negative.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the column index is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the board does not exist.</exception>
        internal void LimitColumn(string boardName, int column, int limit, string email)
        {
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            if (column > 2 || column < 0)
                throw new InvalidOperationException("invalid column");
            if (limit < 0)
                throw new ArgumentException("limit cannot be negative");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.LimitColumn(column, limit, email);
        }

        /// <summary>
        /// Moves a task to the next column.
        /// </summary>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="column">The current column index.</param>
        /// <param name="id">The task ID.</param>
        /// <param name="email">The user's email.</param>
        /// <exception cref="InvalidOperationException">Thrown if the column index is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the board or task does not exist.</exception>
        internal void MoveTask(string boardName, int column, int id, string email)
        {
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            if (column > 1 || column < 0)
                throw new InvalidOperationException("invalid column");
            if (id < 0)
                throw new InvalidOperationException("id can't be negative");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.MoveTask(column, id, email);
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
        /// <exception cref="ArgumentNullException">Thrown if the title is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user doesn't exist or is not logged in ot task id is taken or invalid column.</exception>
        internal void UpdateTask(string boardName, string title, DateTime? due, string description, int id, string email, int column)
        {
            if (title != null && title.Length > 50)
                throw new ArgumentException("title invalid");
            if (description != null && description.Length > 300)
                throw new InvalidOperationException("exceeds limit");
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            if (column > 2 || column < 0)
                throw new InvalidOperationException("invalid column");
            if (id < 0)
                throw new InvalidOperationException("id can't be null");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            board.UpdateTask(title, due, description, id, email, column);
        }

        /// <summary>
        /// Retrieves all tasks in a specified column.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="boardName">The board's name.</param>
        /// <param name="columnOrdinal">The column index.</param>
        /// <returns>A list of <see cref="TaskBL"/>.</returns>
        internal List<TaskBL> GetColumn(string email, string boardName, int columnOrdinal)
        {
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            if (columnOrdinal > 2 || columnOrdinal < 0)
                throw new InvalidOperationException("invalid column");
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
        /// <returns>The task limit.</returns>
        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            if (columnOrdinal > 2 || columnOrdinal < 0)
                throw new InvalidOperationException("invalid column");
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
        /// <returns>The name of the column.</returns>
        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            if (columnOrdinal > 2 || columnOrdinal < 0)
                throw new InvalidOperationException("invalid column");
            UserBL user = _userfacade.GetUser(email);
            BoardBL board = user.GetBoard(boardName);
            return board.GetColumnName(columnOrdinal);
        }

        /// <summary>
        /// Retrieves all in-progress tasks across all boards.
        /// </summary>
        /// <param name="email">The email of the requesting user.</param>
        /// <returns>A list of in-progress <see cref="TaskSL"/> objects.</returns>
        internal List<TaskBL> InProgressTasks(string email)
        {
            if (!UserExistsAndLoggedIn(email))
                throw new InvalidOperationException("user is not logged in or doesn't exist");
            UserBL user = _userfacade.GetUser(email);
            return user.InProgressTasks();
        }
    }
}
