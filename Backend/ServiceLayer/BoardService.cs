using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;

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
        /// <precondition>boardName and email must be non-empty strings.</precondition>
        /// <postcondition>A new board is created and associated with the user.</postcondition>
        public Response<BoardSL> CreateBoard(string boardName, string email)
        {
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Board name and email cannot be null or empty.");
            BoardSL board = _boardFacade.CreateBoard(boardName, email);
            return new Response<BoardSL>("", board);
        }

        /// <summary>
        /// Deletes an existing board for a user.
        /// </summary>
        /// <param name="boardName">The name of the board to delete.</param>
        /// <param name="email">The user's email who owns the board.</param>
        /// <returns>An empty Response indicating success or failure.</returns>
        /// <exception cref="ArgumentNullException">If boardName or email is null or empty.</exception>
        /// <precondition>The board must exist and be owned by the user.</precondition>
        /// <postcondition>The board is removed from the system.</postcondition>
        public Response<object> DeleteBoard(string boardName, string email)
        {
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Board name and email cannot be null or empty.");
            _boardFacade.DeleteBoard(boardName, email);
            return new Response<object>("", null);
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
        /// <precondition>Board and column must exist and belong to the user.</precondition>
        /// <postcondition>The column's task limit is updated.</postcondition>
        public Response<object> LimitColumn(string boardName, int column, int limit, string email)
        {
            if (limit < 0)
                throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be non-negative.");
            _boardFacade.LimitColumn(boardName, column, limit, email);
            return new Response<object>("", null);
        }
    }
}
