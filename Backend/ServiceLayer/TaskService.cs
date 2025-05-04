using log4net;
using log4net.Config;
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
        /// <exception cref="ArgumentNullException">If title is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If trying illegal operations.</exception>
        /// <exception cref="KeyNotFoundException">If board does not exist.</exception>
        /// <precondition>The board must exist and the ID must be unique.</precondition>
        /// <postcondition>The new task is added to the board's backlog.</postcondition>
        public Response<TaskSL> AddTask(string boardName, string title, string due, string description, string creationTime, int id, string email)
        {
            try
            {
                TaskBL task = _boardFacade.AddTask(boardName, title, due, description, creationTime, id, email);
                return new Response<TaskSL>("Task created successfuly", new TaskSL(task));
            }
            catch (Exception ex)
            {
                return new Response<TaskSL>(ex.Message, null);
            }
        }

        /// <summary>
        /// Moves an existing task to the next column.
        /// </summary>
        /// <param name="boardName">Board name.</param>
        /// <param name="column">Current column name.</param>
        /// <param name="id">Task ID.</param>
        /// <param name="email">Owner's email.</param>
        /// <returns>An empty Response.</returns>
        /// <exception cref="InvalidOperationException">If trying illegal opertaions.</exception>
        /// <exception cref="KeyNotFoundException">If boardname or task doesn't exist.</exception>
        /// <precondition>The task must exist in the specified column.</precondition>
        /// <postcondition>The task is moved to the next column.</postcondition>
        public Response<object> MoveTask(string boardName, int column, int id, string email)
        {
            try
            {
                _boardFacade.MoveTask(boardName, column, id, email);
                return new Response<object>("Task moved successfuly", null);
            }
            catch (Exception ex)
            {
                return new Response<object>(ex.Message, null);
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
        /// <exception cref="KeyNotFoundException">If the task ID or boardname does not exist.</exception>
        /// <precondition>The task must exist in the given column.</precondition>
        /// <postcondition>The task fields are updated with the new values.</postcondition>
        public Response<object> UpdateTask(string boardName, string title, string description, string due, int id, string email, int column)
        {
            try
            {
                _boardFacade.UpdateTask(boardName, title, due, description, id, email, column);
                return new Response<object>("task updated", null);
            }
            catch (KeyNotFoundException)
            {
                return new Response<object>("couldn't find key", null);
            }
        }
    }
}
