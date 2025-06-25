using System;
using System.Text.Json;
using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private readonly BoardFacade _boardFacade;

        internal TaskService(BoardFacade boardFacade)
        {
            _boardFacade = boardFacade;
        }

        private string ToJsonResponse(string error = null, object data = null) =>
            JsonSerializer.Serialize(new Response(error, data));

        /// <summary>
        /// Adds a new task to a board's backlog.
        /// </summary>
        /// <param name="boardName">Name of the board.</param>
        /// <param name="title">Title of the task.</param>
        /// <param name="dueDate">Due date of the task.</param>
        /// <param name="description">Task description.</param>
        /// <param name="id">Unique task identifier.</param>
        /// <param name="email">Email of the task owner.</param>
        /// <returns>Serialized response containing the created <see cref="TaskSL"/> or an error message.</returns>
        /// <exception cref="ArgumentNullException">If the title is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If the board or user is in an invalid state for the operation.</exception>
        /// <exception cref="KeyNotFoundException">If the specified board does not exist.</exception>
        /// <precondition>The board must exist and the task ID must be unique within it.</precondition>
        /// <postcondition>The new task is added to the board's backlog.</postcondition>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                TaskBL task = _boardFacade.AddTask(email, boardName, title, description, dueDate);
                return ToJsonResponse(null, new TaskSL(title, description, dueDate, task.CreatedAt, null));
            }
            catch (Exception ex)
            {
                return ToJsonResponse(ex.Message);
            }
        }

        /// <summary>
        /// Moves a task to the next column in the specified board.
        /// </summary>
        /// <param name="boardName">Name of the board.</param>
        /// <param name="columnOrdinal">Index of the current column.</param>
        /// <param name="taskID">ID of the task to move.</param>
        /// <param name="email">Email of the user initiating the move.</param>
        /// <returns>Serialized empty response or error message.</returns>
        /// <exception cref="InvalidOperationException">If the move is not allowed by board rules.</exception>
        /// <exception cref="KeyNotFoundException">If the board or task does not exist.</exception>
        /// <precondition>The task must exist in the specified column and belong to the user.</precondition>
        /// <postcondition>The task is moved to the next column.</postcondition>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskID)
        {
            try
            {
                _boardFacade.AdvanceTask(email, boardName, columnOrdinal, taskID);
                return ToJsonResponse();
            }
            catch (Exception ex)
            {
                return ToJsonResponse(ex.Message);
            }
        }

        /// <summary>
        /// Updates the properties of an existing task.
        /// </summary>
        /// <param name="boardName">Name of the board containing the task.</param>
        /// <param name="title">New title for the task.</param>
        /// <param name="description">New description for the task.</param>
        /// <param name="dueDate">New due date for the task.</param>
        /// <param name="taskID">ID of the task to update.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="columnOrdinal">Index of the column containing the task.</param>
        /// <returns>Serialized empty response or error message.</returns>
        /// <exception cref="KeyNotFoundException">If the board or task is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user doesn't exist or is not logged in ot task id is taken or invalid column.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the title is null or empty.</exception>
        /// <precondition>The task must exist in the given column and belong to the user.</precondition>
        /// <postcondition>The task's title, description, and due date are updated.</postcondition>
        public string UpdateTask(string email, string boardName, int columnOrdinal, int taskID, DateTime? dueDate, string title, string description)
        {
            try
            {
                _boardFacade.UpdateTask(email, boardName, columnOrdinal, taskID, dueDate, title, description);
                return ToJsonResponse();
            }
            catch (Exception ex)
            {
                return ToJsonResponse(ex.Message);
            }
        }
      
        /// <summary>
        /// Assigns a task to another user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskID"></param>
        /// <param name="emailAssignee"></param>
        /// <returns></returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                _boardFacade.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                return ToJsonResponse();
            }
            catch (Exception ex)
            {
                return ToJsonResponse(ex.Message);
            }
        }
    }
}