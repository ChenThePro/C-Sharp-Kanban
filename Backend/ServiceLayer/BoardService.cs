using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private readonly BoardFacade _boardFacade;

        internal BoardService(BoardFacade boardFacade)
        {
            _boardFacade = boardFacade;
        }

        private string ToResponseJson(string errorMessage = null, object returnValue = null) =>
            JsonSerializer.Serialize(new Response(errorMessage, returnValue));

        /// <summary>
        /// Creates a new board for a user.
        /// </summary>
        /// <param name="boardName">The name of the board to be created.</param>
        /// <param name="email">The email of the user creating the board.</param>
        /// <returns>Response containing the created BoardSL object.</returns>
        /// <exception cref="ArgumentNullException">If boardName or email is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If boardName exists.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <precondition>boardName and email must be non-empty strings.</precondition>
        /// <postcondition>A new board is created and associated with the user.</postcondition>
        public string CreateBoard(string email, string boardName)
        {
            try
            {
                _boardFacade.CreateBoard(email, boardName);
                return ToResponseJson();
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing board for a user.
        /// </summary>
        /// <param name="boardName">The name of the board to delete.</param>
        /// <param name="email">The user's email who owns the board.</param>
        /// <returns>An empty Response indicating success or failure.</returns>
        /// <exception cref="InvalidOperationException">.</exception>
        /// <exception cref="KeyNotFoundException">If boardName doesn't exist.</exception>
        /// <precondition>The board must exist and be owned by the user.</precondition>
        /// <postcondition>The board is removed from the system.</postcondition>
        public String DeleteBoard(string email, string boardName)
        {
            try
            {
                _boardFacade.DeleteBoard(email, boardName);
                return ToResponseJson();
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Sets a task limit for a specific column in a board.
        /// </summary>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="column">The name of the column to limit.</param>
        /// <param name="limit">The new limit for the column (must be non-negative).</param>
        /// <param name="email">The user's email.</param>
        /// <returns>An empty Response indicating success or failure.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If limit is negative.</exception>
        /// <exception cref="InvalidOperationException">If column is not valid or number of existing tasks in the column larger than limit.</exception>
        /// <exception cref="KeyNotFoundException">If boardname doexn't exist.</exception>
        /// <precondition>Board and column must exist and belong to the user.</precondition>
        /// <postcondition>The column's task limit is updated.</postcondition>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                _boardFacade.LimitColumn(email, boardName, columnOrdinal, limit);
                return ToResponseJson();
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all tasks in a specific column of a board.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column.</param>
        /// <returns>Response containing a list of TaskBL objects in the column.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">If columnOrdinal is invalid.</exception>
        /// <exception cref="KeyNotFoundException">If the board does not exist or is not accessible by the user.</exception>
        /// <precondition>The user must own the board and the column must exist.</precondition>
        /// <postcondition>A list of tasks in the specified column is returned.</postcondition>
        public string GetColumnTasks(string email, string boardName, int columnOrdinal)
        {
            try
            {
                List<TaskSL> tasks = _boardFacade.GetColumn(email, boardName, columnOrdinal)
                    .Select(t => new TaskSL(t.Title, t.DueDate, t.Description, t.CreatedAt, t.Id)).ToList();
                return ToResponseJson(null, tasks);
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the task limit of a specific column in a board.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column.</param>
        /// <returns>Response containing the column's task limit.</returns>
        /// <exception cref="InvalidOperationException">If any input is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If columnOrdinal is out of range.</exception>
        /// <exception cref="KeyNotFoundException">If the board does not exist or is not owned by the user.</exception>
        /// <precondition>The specified column exists in the user's board.</precondition>
        /// <postcondition>The task limit of the column is returned.</postcondition>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                int limit = _boardFacade.GetColumnLimit(email, boardName, columnOrdinal);
                return ToResponseJson(null, limit);
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the name of a specific column in a board.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column.</param>
        /// <returns>Response containing the name of the column.</returns>
        /// <exception cref="InvalidOperationException">If any input is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If columnOrdinal is out of range.</exception>
        /// <exception cref="KeyNotFoundException">If the board does not exist or is not owned by the user.</exception>
        /// <precondition>The specified column exists in the user's board.</precondition>
        /// <postcondition>The column name is returned.</postcondition>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                string name = _boardFacade.GetColumnName(email, boardName, columnOrdinal);
                return ToResponseJson(null, name);
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all tasks currently in progress for a user.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>Response containing a list of in-progress TaskBL objects.</returns>
        /// <exception cref="InvalidOperationException">If email is null or empty.</exception>
        /// <exception cref="KeyNotFoundException">If no tasks are found or user is invalid.</exception>
        /// <precondition>The user must be registered and have at least one in-progress task.</precondition>
        /// <postcondition>A list of in-progress tasks is returned.</postcondition>
        public string InProgressTasks(string email)
        {
            try
            {
                List<TaskSL> tasks = _boardFacade.InProgressTasks(email)
                    .Select(t => new TaskSL(t.Title, t.DueDate, t.Description, t.CreatedAt, t.Id)).ToList();
                return ToResponseJson(null, tasks);
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Allows a user to join an existing board by its ID.
        /// </summary>
        /// <param name="email">The email of the user who wants to join.</param>
        /// <param name="boardID">The ID of the board.</param>
        /// <returns>Response indicating success or failure.</returns>
        public string JoinBoard(string email, int boardID)
        {
            try
            {
                _boardFacade.JoinBoard(email, boardID);
                return ToResponseJson();
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Allows a user to leave a board they are a member of and removes him from members list of a board.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="boardID">The ID of the board.</param>
        /// <returns>Response indicating success or failure.</returns>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                _boardFacade.LeaveBoard(email, boardID);
                return ToResponseJson();
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Gets the name of a board by its ID.
        /// </summary>
        /// <param name="boardId">The ID of the board.</param>
        /// <returns>Response containing the board name.</returns>
        public string GetBoardName(int boardID)
        {
            try
            {
                string boardName = _boardFacade.GetBoardName(boardID);
                return ToResponseJson(null, boardName);
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>Response containing a list of board Ids.</returns>
        public string GetUserBoardsAsId(string email)
        {
            try
            {
                List<int> boardIds = _boardFacade.GetUserBoardsAsId(email);
                return ToResponseJson(null, boardIds);
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Transfers ownership of a board to another user.
        /// </summary>
        /// <param name="currentOwnerEmail">The email of the current board owner.</param>
        /// <param name="newOwnerEmail">The email of the new board owner.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <returns>Response indicating success or failure.</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                _boardFacade.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                return ToResponseJson();
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }

        /// <summary>
        /// Loads all data from DataBase to the system
        /// </summary>
        /// <returns>Response indicating success or failure</returns>
        public string LoadData()
        {
            try
            {
                _boardFacade.LoadData();
                return ToResponseJson();
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }

        }

        /// <summary>
        /// Loads all data from DataBase to the system
        /// </summary>
        /// <returns>Response indicating success or failure</returns>
        public string DeleteData()
        {
            try
            {
                _boardFacade.DeleteData();
                return ToResponseJson();
            }
            catch (Exception ex)
            {
                return ToResponseJson(ex.Message);
            }
        }
    }
}