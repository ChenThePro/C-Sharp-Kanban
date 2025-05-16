using System.Collections.Immutable;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
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
            string json = _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
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


        public bool CheckOwnership()
        {
            _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            string json = _factory.GetBoardService().TransferOwnership(_userEmail,"noa@gmail.com", _boardName);
            return !json.Contains("\"ErrorMessage\":null");
        }
        /// <summary>
        /// to change the id to the board id!!!!!!!!!!!!!!!!
        /// </summary>
        /// <returns></returns>
        public bool CheckJoiningToANewBoard()
        {
            _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            string json = _factory.GetBoardService().JoinBoard(_userEmail, 1);
            return !json.Contains("\"ErrorMessage\":null");

        }

        public bool checkUserBoardIdList()
        {
            _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            string json = _factory.GetBoardService().GetUserBoards(_userEmail);
            return !json.Contains("\"ErrorMessage\":null");

        }
        public bool checkUserBoardIdList_userWithoutBoards()
        {
            _factory.GetUserService().Register(_userEmail,"Aa123456");
            string json = _factory.GetBoardService().GetUserBoards(_userEmail);
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
        }


    }
}