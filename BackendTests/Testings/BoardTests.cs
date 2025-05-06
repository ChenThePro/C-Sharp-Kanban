using Backend.ServiceLayer;
using System.Text.Json;

namespace Backend.BackendTests.Testings
{
    public class BoardTests
    {
        private readonly ServiceFactory _factory = new ServiceFactory();

        public bool CreateBoardWithValidValues()
        {
            string userEmail = "test@example.com";
            string boardName = "My First Board";
            _factory.GetUserService().Register(userEmail, "Password1");
            var json = _factory.GetBoardService().CreateBoard(boardName, userEmail);
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg == null;
        }

        public bool CreateBoardWithOccupiedName()
        {
            string userEmail = "test@example.com";
            string boardName = "My First Board";
            var json = _factory.GetBoardService().CreateBoard(boardName, userEmail);
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg != null;
        }

        public bool CreateBoard_CaseInsensitiveName()
        {
            string userEmail = "test@example.com";
            string boardName = "my first board";
            var json = _factory.GetBoardService().CreateBoard(boardName, userEmail);
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg != null;
        }

        public bool DeleteBoard()
        {
            string userEmail = "test@example.com";
            string boardName = "My First Board";
            var json = _factory.GetBoardService().DeleteBoard(boardName, userEmail);
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg == null;
        }

        public bool DeleteNonExistentBoard()
        {
            string userEmail = "test@example.com";
            var json = _factory.GetBoardService().DeleteBoard("NonExistentBoard", userEmail);
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg != null;
        }

        public void RunAll()
        {
            Console.WriteLine("🔹 CreateBoardWithValidValues: " + CreateBoardWithValidValues());
            Console.WriteLine("🔹 CreateBoardWithOccupiedName: " + CreateBoardWithOccupiedName());
            Console.WriteLine("🔹 CreateBoard_CaseInsensitiveName: " + CreateBoard_CaseInsensitiveName());
            Console.WriteLine("🔹 DeleteBoard: " + DeleteBoard());
            Console.WriteLine("🔹 DeleteNonExistentBoard: " + DeleteNonExistentBoard());
        }
    }
}
