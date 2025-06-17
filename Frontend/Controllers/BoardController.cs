using Frontend.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class BoardController
    {
        private readonly BoardService _boardService;

        public BoardController(BoardService boardService) => _boardService = boardService;

        public BoardModel CreateBoard(string email, string boardName) =>
            new(this, Call<BoardSL>(() => _boardService.CreateBoard(email, boardName)));

        public void DeleteBoard(string email, string boardName) =>
            Call<object>(() => _boardService.DeleteBoard(email, boardName));

        private T Call<T>(Func<string> serviceCall)
        {
            var response = JsonSerializer.Deserialize<Response>(serviceCall())!;
            if (!string.IsNullOrEmpty(response.ErrorMessage))
                throw new(response.ErrorMessage);
            return response.ReturnValue is null ? default! : ((JsonElement)response.ReturnValue).Deserialize<T>()!;
        }
    }
}