using System.Collections.Immutable;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
{
    public class BoardTests
    {
        private string _userEmail = "test@example.com";
        private string _boardName = "My First Board";
        private readonly ServiceFactory _factory = new ServiceFactory();

        public bool CreateBoardWithValidValues()
        {
            _factory.GetUserService().Register(_userEmail, "Password1");
            string json = _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool CreateBoardWithTheSameNmaeForDifferentUsers()
        {
            _factory.GetUserService().Register("kuku@gmail.com", "Password1");
            string json = _factory.GetBoardService().CreateBoard(_boardName, "kuku@gmail.com");
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool CreateBoardWithOccupiedName()
        {
            string json = _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool CreateBoard_CaseInsensitiveName()
        {
            string json = _factory.GetBoardService().CreateBoard("my first board", _userEmail);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool DeleteBoard()
        {
            string json = _factory.GetBoardService().DeleteBoard(_boardName, _userEmail);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool DeleteNonExistentBoard()
        {
            string json = _factory.GetBoardService().DeleteBoard("NonExistentBoard", _userEmail);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool CheckJoiningToANewBoard()
        {
            _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            string json = _factory.GetBoardService().JoinBoard("kuku@gmail.com", 3);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool CheckOwnership()
        {
            string json = _factory.GetBoardService().TransferOwnership(_userEmail, "kuku@gmail.com", _boardName);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool CheckUserBoardIdList()
        {
            string json = _factory.GetBoardService().GetUserBoards(_userEmail);
            return !json.Contains("\"ErrorMessage\":null");
        }
        public bool CheckUserBoardIdList_userWithoutBoards()
        {
            _factory.GetUserService().Register("noa@gmail.com", "Aa123456");
            string json = _factory.GetBoardService().GetUserBoards("noa@gmail.com");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public void RunAll()
        {
            Console.WriteLine("🔹 CreateBoardWithValidValues: " + CreateBoardWithValidValues());
            Console.WriteLine("🔹 CreateBoardWithTheSameNmaeForDifferentUsers: " + CreateBoardWithTheSameNmaeForDifferentUsers());
            Console.WriteLine("🔹 CreateBoardWithOccupiedName: " + CreateBoardWithOccupiedName());
            Console.WriteLine("🔹 CreateBoard_CaseInsensitiveName: " + CreateBoard_CaseInsensitiveName());
            Console.WriteLine("🔹 DeleteBoard: " + DeleteBoard());
            Console.WriteLine("🔹 DeleteNonExistentBoard: " + DeleteNonExistentBoard());
            Console.WriteLine("🔹 CheckJoiningToANewBoard: " + CheckJoiningToANewBoard());
            Console.WriteLine("🔹 CheckOwnership: " + CheckOwnership());
            Console.WriteLine("🔹 CheckUserBoardIdList: " + CheckUserBoardIdList());
            Console.WriteLine("🔹 CheckUserBoardIdList_userWithoutBoards: " + CheckUserBoardIdList_userWithoutBoards());
        }
    }
}