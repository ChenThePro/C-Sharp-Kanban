using System;
using System.Text.Json;
using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private readonly BoardFacade _boardFacade;

        private int _id = 1;


        internal TaskService(BoardFacade boardFacade)
        {
            _boardFacade = boardFacade;
        }

        /// <summary>
        /// Adds a new task to a board's backlog.
        /// </summary>
        /// <param name="boardName">Name of the board.</param>
        /// <param name="title">Title of the task.</param>
        /// <param name="due">Due date of the task.</param>
        /// <param name="description">Task description.</param>
        /// <param name="creationTime">Task creation timestamp.</param>
        /// <param name="id">Unique task identifier.</param>
        /// <param name="email">Email of the task owner.</param>
        /// <returns>Serialized response containing the created <see cref="TaskSL"/> or an error message.</returns>
        /// <exception cref="ArgumentNullException">If the title is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If the board or user is in an invalid state for the operation.</exception>
        /// <exception cref="KeyNotFoundException">If the specified board does not exist.</exception>
        /// <precondition>The board must exist and the task ID must be unique within it.</precondition>
        /// <postcondition>The new task is added to the board's backlog.</postcondition>
        public string AddTask(string boardName, string title, DateTime due, string description, DateTime creationTime, string email)
        {
            try
            {
                int id = _id;
                TaskBL task = _boardFacade.AddTask(boardName, title, due, description, creationTime, id, email);
                _id++;
                return JsonSerializer.Serialize(new Response(null, null));
                
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
        }

        /// <summary>
        /// Moves a task to the next column in the specified board.
        /// </summary>
        /// <param name="boardName">Name of the board.</param>
        /// <param name="column">Index of the current column.</param>
        /// <param name="id">ID of the task to move.</param>
        /// <param name="email">Email of the user initiating the move.</param>
        /// <returns>Serialized empty response or error message.</returns>
        /// <exception cref="InvalidOperationException">If the move is not allowed by board rules.</exception>
        /// <exception cref="KeyNotFoundException">If the board or task does not exist.</exception>
        /// <precondition>The task must exist in the specified column and belong to the user.</precondition>
        /// <postcondition>The task is moved to the next column.</postcondition>
        public string MoveTask(string boardName, int column, int id, string email)
        {
            try
            {
                _boardFacade.MoveTask(boardName, column, id, email);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
        }

        /// <summary>
        /// Updates the properties of an existing task.
        /// </summary>
        /// <param name="boardName">Name of the board containing the task.</param>
        /// <param name="title">New title for the task.</param>
        /// <param name="description">New description for the task.</param>
        /// <param name="due">New due date for the task.</param>
        /// <param name="id">ID of the task to update.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="column">Index of the column containing the task.</param>
        /// <returns>Serialized empty response or error message.</returns>
        /// <exception cref="KeyNotFoundException">If the board or task is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user doesn't exist or is not logged in ot task id is taken or invalid column.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the title is null or empty.</exception>
        /// <precondition>The task must exist in the given column and belong to the user.</precondition>
        /// <postcondition>The task's title, description, and due date are updated.</postcondition>
        public string UpdateTask(string boardName, string title, string description, DateTime? due, int id, string email, int column)
        {
            try
            {
                _boardFacade.UpdateTask(boardName, title, due, description, id, email, column);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
        }
    }
}