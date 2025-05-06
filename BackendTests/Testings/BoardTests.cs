using Backend.ServiceLayer;
using System.Text.Json;

namespace Backend.BackendTests.Testings
{
    public class BoardTests
    {
        private readonly ServiceFactory _factory = new ServiceFactory();
        private string _userEmail = "test@example.com";
        private string _boardName = "My First Board";

        public bool CreateBoardWithValidValues()
        {
            _factory.GetUserService().Register(_userEmail, "Password1");
            string json = _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg == null;
        }

        public bool CreateBoardWithOccupiedName()
        {
            string json = _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool CreateBoard_CaseInsensitiveName()
        {
            string json = _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
        }

        public bool DeleteBoard()
        {
            string json = _factory.GetBoardService().DeleteBoard(_boardName, _userEmail);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg == null;
        }

        public bool DeleteNonExistentBoard()
        {
            string json = _factory.GetBoardService().DeleteBoard("NonExistentBoard", _userEmail);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg != null;
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
