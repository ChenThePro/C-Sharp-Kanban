using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.ServiceLayer;

namespace Tests
{
    public class BoardTests
    {
        private ServiceFactory _factory;

        /// <summary>
        /// Test creating a board with valid values.
        /// Preconditions: The user exists and the board name is unique.
        /// Postconditions: The board is created successfully.
        /// Throws: None.
        /// </summary>
        public bool CreateBoardWithValidValues()
        {
            string userEmail = "test@example.com";
            string boardName = "My First Board";
            Response response = _factory.Bs.CreateBoard(userEmail, boardName);
            if (response.ErrorMsg != null || response.RetVal == null)
                return false;
            return true;
        }

        /// <summary>
        /// Test creating a board with a name that already exists.
        /// Preconditions: A board with the same name already exists for the user.
        /// Postconditions: The second creation attempt fails.
        /// Throws: None.
        /// </summary>
        public bool CreateBoardWithOccupiedName()
        {
            string userEmail = "test@example.com";
            string boardName = "My First Board";
            _factory.Bs.CreateBoard(userEmail, boardName);
            Response response = _factory.Bs.CreateBoard(userEmail, boardName);
            if (response.ErrorMsg == null || response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test creating boards with case-insensitive name collision.
        /// Preconditions: The user already has a board with the same name differing only in case.
        /// Postconditions: The creation should fail due to name conflict.
        /// Throws: None.
        /// </summary>
        public bool CreateBoard_CaseInsensitiveName()
        {
            string userEmail = "test@example.com";
            string boardName1 = "Board";
            string boardName2 = "board";
            _factory.Bs.CreateBoard(userEmail, boardName1);
            Response response = _factory.Bs.CreateBoard(userEmail, boardName2);
            if (response.ErrorMsg == null || response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test deleting a board.
        /// Preconditions: The board exists.
        /// Postconditions: The board is removed from the user's list.
        /// Throws: None.
        /// </summary>
        public bool DeleteBoard()
        {
            string userEmail = "test@example.com";
            string boardName = "Board";
            _factory.Bs.CreateBoard(userEmail, boardName);
            Response deleteResponse = _factory.Bs.DeleteBoard(userEmail, boardName);
            Response boardsResponse = _factory.Bs.GetBoards(userEmail); // correct but probably will be in Us and not Bs
            if (deleteResponse.ErrorMsg != null)
                return false;
            if (((List<string>)boardsResponse.ReturnValue).Contains(boardName))
                return false;
            return true;
        }

        /// <summary>
        /// Test deleting a non-existent board.
        /// Preconditions: The board does not exist.
        /// Postconditions: The delete attempt fails.
        /// Throws: None.
        /// </summary>
        public bool DeleteNonExistentBoard()
        {
            string userEmail = "test@example.com";
            Response response = _factory.Bs.DeleteBoard(userEmail, "NonExistentBoard");
            if (response.ErrorMsg == null)
                return false;
            return true;
        }

        /// <summary>
        /// Test creating boards with the same name for different users.
        /// Preconditions: Two users exist with different emails.
        /// Postconditions: Both can create boards with the same name.
        /// Throws: None.
        /// </summary>
        public bool CreateBoardsSameNameForDifferentUsers()
        {
            string userEmail1 = "test1@example.com";
            string userEmail2 = "test2@example.com";
            string boardName = "Board";
            _factory.Bs.CreateBoard(userEmail1, boardName);
            Response response = _factory.Bs.CreateBoard(userEmail2, boardName);
            if (response.ErrorMessage != null || response.ReturnValue == null)
                return false;
            return true;
        }

        public void RunAll()
        {
            Console.WriteLine("🔹 CreateBoardWithValidValues: " + CreateBoardWithValidValues());
            Console.WriteLine("🔹 CreateBoardWithOccupiedName: " + CreateBoardWithOccupiedName());
            Console.WriteLine("🔹 CreateBoard_CaseInsensitiveName: " + CreateBoard_CaseInsensitiveName());
            Console.WriteLine("🔹 DeleteBoard: " + DeleteBoard());
            Console.WriteLine("🔹 DeleteNonExistentBoard: " + DeleteNonExistentBoard());
            Console.WriteLine("🔹 CreateBoardsSameNameForDifferentUsers: " + CreateBoardsSameNameForDifferentUsers());
        }
    }
}