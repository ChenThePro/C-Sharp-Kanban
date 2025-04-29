using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;

namespace Backend.ServiceLayer
{
    public class TaskService
    {
        private readonly BoardFacade _boardFacade;

        internal TaskService(BoardFacade boardFacade)
        {
            _boardFacade = boardFacade;
        }

        /// <summary>
        /// Adds a new task to a board's backlog.
        /// </summary>
        /// <param name="boardName">Board name.</param>
        /// <param name="title">Task title.</param>
        /// <param name="description">Task description.</param>
        /// <param name="due">Due date (ISO 8601 format).</param>
        /// <param name="id">Unique task identifier.</param>
        /// <param name="email">Owner's email.</param>
        /// <returns>Response containing the created TaskSL.</returns>
        /// <exception cref="ArgumentNullException">If any required parameter is null or empty.</exception>
        /// <precondition>The board must exist and the ID must be unique.</precondition>
        /// <postcondition>The new task is added to the board's backlog.</postcondition>
        public Response<TaskSL> AddTask(string boardName, string title, string description, string due, int id, string email)
        {
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Board name, title, or email cannot be null or empty.");
            TaskSL task = _boardFacade.AddTask(boardName, title, description, due, id, email);
            return new Response<TaskSL>("", task);
        }

        /// <summary>
        /// Moves an existing task to the next column.
        /// </summary>
        /// <param name="boardName">Board name.</param>
        /// <param name="column">Current column name.</param>
        /// <param name="id">Task ID.</param>
        /// <param name="email">Owner's email.</param>
        /// <returns>An empty Response.</returns>
        /// <exception cref="InvalidOperationException">If task cannot be moved (e.g. already in Done).</exception>
        /// <precondition>The task must exist in the specified column.</precondition>
        /// <postcondition>The task is moved to the next column.</postcondition>
        public Response<object> MoveTask(string boardName, string column, int id, string email)
        {
            try
            {
                _boardFacade.MoveTask(boardName, column, id, email);
                return new Response<object>("", null);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates an existing task's details.
        /// </summary>
        /// <param name="boardName">Board name.</param>
        /// <param name="title">New task title.</param>
        /// <param name="description">New description.</param>
        /// <param name="due">New due date.</param>
        /// <param name="id">Task ID.</param>
        /// <param name="email">Owner's email.</param>
        /// <param name="column">Column containing the task.</param>
        /// <returns>An empty Response.</returns>
        /// <exception cref="KeyNotFoundException">If the task ID does not exist.</exception>
        /// <precondition>The task must exist in the given column.</precondition>
        /// <postcondition>The task fields are updated with the new values.</postcondition>
        public Response<object> UpdateTask(string boardName, string title, string description, string due, int id, string email, string column)
        {
            try
            {
                _boardFacade.UpdateTask(boardName, title, description, due, id, email, column);
                return new Response<object>("", null);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }
    }
}
