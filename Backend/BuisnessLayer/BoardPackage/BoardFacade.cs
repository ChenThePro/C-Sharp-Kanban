using log4net;

namespace Backend.BuisnessLayer.BoardPackage
{
    internal class BoardFacade
    {
        private readonly Dictionary<string, BoardBL> boards;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        internal BoardFacade()
        {
            boards = new Dictionary<string, BoardBL>();
        }

        /// <summary>
        /// helper function to check if board exist
        /// </summary>
        /// <param name="boardName"></param>
        /// <returns></returns>
        private bool BoardExists(string boardName)
        {
            return boards.ContainsKey(boardName);
        }

        /// <summary>
        /// helper function to get board by name;
        /// </summary>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        private BoardBL GetBoardByName(string boardName)
        {
            if (!BoardExists(boardName))
                throw new KeyNotFoundException("Board not found");
            return boards[boardName];
        }

        /// <summary>
        /// helper function to check if task name already exists in this board;
        /// </summary>
        /// <param name="boardName"></param>
        /// <param name="id"></param>
        /// <param name="column"></param>
        /// <returns></returns>
       private TaskBL? GetTaskByIdAndColumn(string boardName, int id, int column)
       {
            BoardBL board = GetBoardByName(boardName);
            return board.GetTaskByIdAndColumn(id, column);
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
        /// <exception cref="Exception"></exception>
        internal TaskBL AddTask(string boardName, string title, string due, string description, string creatinTime, int id, string email)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException("title cannot be null or empty");
            if (GetTaskByIdAndColumn(boardName, id, 0) != null || GetTaskByIdAndColumn(boardName, id, 1) != null || GetTaskByIdAndColumn(boardName, id, 2) != null)
                throw new InvalidOperationException("task id is already taken in this board");
            return boards[boardName].AddTask(title, due, description, creatinTime, id, email, 0);
           
        }

        /// <summary>
        /// Creates a new board with the specified name and owner email.
        /// </summary>
        /// <param name="boardName">The name of the board to create.</param>
        /// <param name="email">The email of the board's creator.</param>
        /// <returns>The created BoardBL object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the board name or email is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a board with the given name already exists.</exception>
        internal BoardBL CreateBoard(string boardName, string email)
        {
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Board name or email cannot be null or empty.");
            if (BoardExists(boardName))
                throw new InvalidOperationException("Board already exists");
            boards.Add(boardName, new BoardBL(boardName));
            Log.Info("new board created - " + boardName);
            return boards[boardName];
            
        }

        /// <summary>
        /// Deletes an existing board with the specified name.
        /// </summary>
        /// <param name="boardName">The name of the board to delete.</param>
        /// <param name="email">The email of the user attempting to delete the board.</param>
        /// <exception cref="ArgumentNullException">Thrown when the board name or email is null or empty.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the specified board does not exist.</exception>
        internal void DeleteBoard(string boardName, string email)
        {
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Board name or email cannot be null or empty.");
            if (!BoardExists(boardName))
                throw new KeyNotFoundException("Board not found");
            boards.Remove(boardName);
            Log.Info(boardName + "deleted");
        }

        /// <summary>
        /// Sets a task limit for a specific column in a board.
        /// </summary>
        /// <param name="boardName">The name of the board containing the column.</param>
        /// <param name="column">The column index (0 = Backlog, 1 = In Progress, 2 = Done).</param>
        /// <param name="limit">The maximum number of tasks allowed in the column.</param>
        /// <param name="email">The email of the user applying the limit.</param>
        /// <exception cref="InvalidOperationException">Thrown when the column index is out of range.</exception>
        /// <exception cref="ArgumentException">Thrown when the limit is negative.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the specified board does not exist.</exception>
        internal void LimitColumn(string boardName, int column, int limit, string email)
        {
            if (column >= 3 || column < 0)
                throw new InvalidOperationException("invalid column");
            if (limit < 0)
                throw new ArgumentException("limit cannot be negative");
            BoardBL board = GetBoardByName(boardName);
            board.LimitColumn(column, limit, email);
        }

        /// <summary>
        /// Moves a task from its current column to the next column in the workflow.
        /// </summary>
        /// <param name="boardName">The name of the board containing the task.</param>
        /// <param name="column">The current column of the task (0=Backlog, 1=In Progress).</param>
        /// <param name="id">The unique identifier of the task to move.</param>
        /// <param name="email">The email of the user attempting to move the task.</param>
        /// <exception cref="InvalidOperationException">Thrown when the column is invalid or exceeds limit.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the board or task does not exist.</exception>
        internal void MoveTask(string boardName, int column, int id, string email)
        {
            if (column >= 2 || column < 0)
                throw new InvalidOperationException("invalid column");
            BoardBL board = GetBoardByName(boardName);
            board.MoveTask(column, id, email);
        }

        /// <summary>
        /// Updates the details of an existing task.
        /// </summary>
        /// <param name="boardName">The name of the board containing the task.</param>
        /// <param name="title">The new title for the task.</param>
        /// <param name="due">The new due date for the task.</param>
        /// <param name="description">The new description for the task.</param>
        /// <param name="id">The unique identifier of the task to update.</param>
        /// <param name="email">The email of the user attempting to update the task.</param>
        /// <param name="column">The column where the task is located (0=Backlog, 1=In Progress, 2=Done).</param>
        /// <exception cref="ArgumentNullException">Thrown when the title is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the task ID is already taken.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the board or task does not exist.</exception>
        internal void UpdateTask(string boardName, string title, string due, string description, int id, string email, int column)
        {
            if (column >= 2 || column < 0)
                throw new InvalidOperationException("invalid column");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException("title cannot be null or empty");
            if (GetTaskByIdAndColumn(boardName, id, 0) != null || GetTaskByIdAndColumn(boardName, id, 1) != null || GetTaskByIdAndColumn(boardName, id, 2) != null)
                throw new InvalidOperationException("task id is already taken in this board");
            BoardBL board = GetBoardByName(boardName);
            board.UpdateTask(title, due, description, id, email, column);
        }

        internal List<TaskBL> GetColumn(string email, string boardName, int columnOrdinal)
        {
            BoardBL board = GetBoardByName(boardName);
            return board.GetColumn(columnOrdinal);

        }

        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            BoardBL board = GetBoardByName(boardName);
            return board.GetColumnLimit(columnOrdinal);
        }

        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            BoardBL board = GetBoardByName(boardName);
            return board.GetColumnName(columnOrdinal);
        }

        internal List<TaskBL> InProgressTasks(string email)
        {
            List<TaskBL> lst = new List<TaskBL>();
            foreach (BoardBL board in boards.Values)
                lst.AddRange(board.InProgressTask());
            return lst;
        }
    }
}
