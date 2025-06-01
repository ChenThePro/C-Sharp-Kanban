using System.Collections.Immutable;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
{
    public class BoardTests
    {
        private ServiceFactory _factory = null!;
        private const string EMAIL1 = "test1@gmail.com";
        private const string EMAIL2 = "test2@gmail.com";
        private const string PASSWORD = "Password1";
        private const string BOARDNAME1 = "My First Board";
        private const string BOARDNAME2 = "My Second Board";

        public bool CreateBoardWithValidValues()
        {
            string json = _factory.GetBoardService().CreateBoard(EMAIL1, BOARDNAME1);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool CreateBoardWithTheSameNmaeForDifferentUsers()
        {
            _factory.GetUserService().Login(EMAIL2, PASSWORD);
            string json = _factory.GetBoardService().CreateBoard(EMAIL2, BOARDNAME1);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool CreateBoardWithOccupiedName()
        {
            string json = _factory.GetBoardService().CreateBoard(EMAIL1, BOARDNAME1);
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool CreateBoard_CaseInsensitiveName()
        {
            string json = _factory.GetBoardService().CreateBoard(EMAIL1, "my first board");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool DeleteBoard()
        {
            string json = _factory.GetBoardService().DeleteBoard(EMAIL1, BOARDNAME1);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool DeleteNonExistentBoard()
        {
            string json = _factory.GetBoardService().DeleteBoard(EMAIL1, "NonExistentBoard");
            return !json.Contains("\"ErrorMessage\":null");
        }

        public bool CheckJoiningToANewBoard()
        {
            _factory.GetBoardService().CreateBoard(EMAIL1, BOARDNAME2);
            string json = _factory.GetBoardService().JoinBoard(EMAIL2, 3);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool CheckTransferOwnership()
        {
            string json = _factory.GetBoardService().TransferOwnership(EMAIL1, EMAIL2, BOARDNAME2);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool CheckUserBoardIdList()
        {
            string json = _factory.GetBoardService().GetUserBoardsAsId(EMAIL2);
            return json.Contains("\"ErrorMessage\":null");
        }
        public bool CheckUserBoardIdList_userWithoutBoards()
        {
            _factory.GetUserService().Register("empty@gmail.com", "Empty1");
            string json = _factory.GetBoardService().GetUserBoardsAsId("empty@gmail.com");
            return json.Contains("\"ErrorMessage\":null");
        }

        public void RunAll(ServiceFactory serviceFactory)
        {
            _factory = serviceFactory;
            Console.WriteLine("🔹 CreateBoardWithValidValues: " + CreateBoardWithValidValues());
            Console.WriteLine("🔹 CreateBoardWithTheSameNmaeForDifferentUsers: " + CreateBoardWithTheSameNmaeForDifferentUsers());
            Console.WriteLine("🔹 CreateBoardWithOccupiedName: " + CreateBoardWithOccupiedName());
            Console.WriteLine("🔹 CreateBoard_CaseInsensitiveName: " + CreateBoard_CaseInsensitiveName());
            Console.WriteLine("🔹 DeleteBoard: " + DeleteBoard());
            Console.WriteLine("🔹 DeleteNonExistentBoard: " + DeleteNonExistentBoard());
            Console.WriteLine("🔹 CheckJoiningToANewBoard: " + CheckJoiningToANewBoard());
            Console.WriteLine("🔹 CheckTransferOwnership: " + CheckTransferOwnership());
            Console.WriteLine("🔹 CheckUserBoardIdList: " + CheckUserBoardIdList());
            Console.WriteLine("🔹 CheckUserBoardIdList_userWithoutBoards: " + CheckUserBoardIdList_userWithoutBoards());
        }
    }
}