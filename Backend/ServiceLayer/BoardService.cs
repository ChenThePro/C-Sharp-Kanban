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
                return JsonSerializer.Serialize(new Response<BoardSL>("Board Created", new BoardSL(board)));
            }
            catch (Exception ex) {
                return JsonSerializer.Serialize(new Response<BoardSL>(ex.Message, null));
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
                return JsonSerializer.Serialize(new Response<object>("Board Deleted", null));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response<object>(ex.Message, null));
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
                return JsonSerializer.Serialize(new Response<object>("Column Limited", null));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response<object>(ex.Message, null));
            }
        }
    }
}
