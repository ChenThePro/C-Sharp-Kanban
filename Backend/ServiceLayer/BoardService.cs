using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Backend.BuisnessLayer.BoardPackage;

namespace Backend.ServiceLayer
{
    public class BoardService
    {
        private readonly BoardFacade _boardFacade;

        internal BoardService(BoardFacade boardFacade)
        {
            _boardFacade = boardFacade;
        }

        /// <summary>
        /// Creates a new board for a user.
        /// </summary>
        /// <param name="boardName">The name of the board to be created.</param>
        /// <param name="email">The email of the user creating the board.</param>
        /// <returns>Response containing the created BoardSL object.</returns>
        /// <exception cref="ArgumentNullException">If boardName or email is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If boardName exists.</exception>
        /// <precondition>boardName and email must be non-empty strings.</precondition>
        /// <postcondition>A new board is created and associated with the user.</postcondition>
        public string CreateBoard(string boardName, string email)
        {
            try
            {
                BoardBL board = _boardFacade.CreateBoard(boardName, email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// Deletes an existing board for a user.
        /// </summary>
        /// <param name="boardName">The name of the board to delete.</param>
        /// <param name="email">The user's email who owns the board.</param>
        /// <returns>An empty Response indicating success or failure.</returns>
        /// <exception cref="ArgumentNullException">If boardName or email is null or empty.</exception>
        /// <exception cref="KeyNotFoundException">If boardName doesn't exist.</exception>
        /// <precondition>The board must exist and be owned by the user.</precondition>
        /// <postcondition>The board is removed from the system.</postcondition>
        public String DeleteBoard(string boardName, string email)
        {
            try
            {
                _boardFacade.DeleteBoard(boardName, email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
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
        /// <exception cref="ArgumentException">If limit is negative.</exception>
        /// <exception cref="InvalidOperationException">If column is not valid or number of existing tasks in the column larger than limit.</exception>
        /// <exception cref="KeyNotFoundException">If boardname doexn't exist.</exception>
        /// <precondition>Board and column must exist and belong to the user.</precondition>
        /// <postcondition>The column's task limit is updated.</postcondition>
        public string LimitColumn(string boardName, int column, int limit, string email)
        {
            try
            {
                _boardFacade.LimitColumn(boardName, column, limit, email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all tasks in a specific column of a board.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column.</param>
        /// <returns>Response containing a list of TaskBL objects in the column.</returns>
        /// <exception cref="ArgumentNullException">If any input is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If columnOrdinal is invalid.</exception>
        /// <exception cref="KeyNotFoundException">If the board does not exist or is not accessible by the user.</exception>
        /// <precondition>The user must own the board and the column must exist.</precondition>
        /// <postcondition>A list of tasks in the specified column is returned.</postcondition>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                List<TaskSL> lst = (List<TaskSL>) _boardFacade.GetColumn(email, boardName, columnOrdinal).Select(task => new TaskSL(task.title, task.due, task.description, task.creationTime, task.id));
                return JsonSerializer.Serialize(new Response(lst));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }


        /// <summary>
        /// Retrieves the task limit of a specific column in a board.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column.</param>
        /// <returns>Response containing the column's task limit.</returns>
        /// <exception cref="ArgumentNullException">If any input is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If columnOrdinal is out of range.</exception>
        /// <exception cref="KeyNotFoundException">If the board does not exist or is not owned by the user.</exception>
        /// <precondition>The specified column exists in the user's board.</precondition>
        /// <postcondition>The task limit of the column is returned.</postcondition>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                int limit = _boardFacade.GetColumnLimit(email, boardName, columnOrdinal);
                return JsonSerializer.Serialize(new Response(limit));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// Retrieves the name of a specific column in a board.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column.</param>
        /// <returns>Response containing the name of the column.</returns>
        /// <exception cref="ArgumentNullException">If any input is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If columnOrdinal is out of range.</exception>
        /// <exception cref="KeyNotFoundException">If the board does not exist or is not owned by the user.</exception>
        /// <precondition>The specified column exists in the user's board.</precondition>
        /// <postcondition>The column name is returned.</postcondition>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                string name = _boardFacade.GetColumnName(email, boardName, columnOrdinal);
                return JsonSerializer.Serialize(new Response(name, true));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all tasks currently in progress for a user.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>Response containing a list of in-progress TaskBL objects.</returns>
        /// <exception cref="ArgumentNullException">If email is null or empty.</exception>
        /// <exception cref="KeyNotFoundException">If no tasks are found or user is invalid.</exception>
        /// <precondition>The user must be registered and have at least one in-progress task.</precondition>
        /// <postcondition>A list of in-progress tasks is returned.</postcondition>
        public string InProgressTasks(string email)
        {
            try
            {
                List<TaskSL> lst = (List<TaskSL>) _boardFacade.InProgressTasks(email).Select(task => new TaskSL(task.title, task.due, task.description, task.creationTime, task.id));
                return JsonSerializer.Serialize(new Response(lst));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }
    }
}