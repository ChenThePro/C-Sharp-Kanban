using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class BoardTests
    {
        private ServiceFactory _factory = new ServiceFactory(new BoardFacade(), new UserFacade());

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
            _factory.GetUserService().Register(userEmail, "Password1");
            Response<BoardSL> response = _factory.GetBoardService().CreateBoard(boardName, userEmail);
            if (response.RetVal == null)
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
            Response<BoardSL> response = _factory.GetBoardService().CreateBoard(boardName, userEmail);
            if (response.RetVal != null)
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
            string boardName = "my first board";
            Response<BoardSL> response = _factory.GetBoardService().CreateBoard(boardName, userEmail);
            if (response.RetVal != null)
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
            string boardName = "My First Board";
            Response<object> deleteResponse = _factory.GetBoardService().DeleteBoard(boardName, userEmail);
            if (deleteResponse.ErrorMsg != null)
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
            Response<object> response = _factory.GetBoardService().DeleteBoard("NonExistentBoard", userEmail);
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
            _factory.GetBoardService().CreateBoard(boardName, userEmail1);
            Response<BoardSL> response = _factory.GetBoardService().CreateBoard(boardName, userEmail2);
            if (response.RetVal == null)
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