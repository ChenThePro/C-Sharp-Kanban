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


        /// <summary>
        /// Test moving a task with valid parameters.
        /// Preconditions: Task exists in board and not in last column.
        /// Postconditions: Task is moved to the next column.
        /// Throws: None.
        /// </summary>
        public bool MoveTask_Valid()
        {
            string email = "test@example.com";
            string boardName = "Board";
            _factory.Bs.CreateBoard(email, boardName);
            var addResp = _factory.Ts.AddTask(email, boardName, "Title", "Desc", DateTime.Now.AddDays(2));
            if (addResp.ErrorMsg != null) return false;

            int taskId = (int)addResp.ReturnValue;
            var moveResp = _factory.Ts.MoveTask(email, boardName, 0, taskId);
            return moveResp.ErrorMsg == null;
        }

        /// <summary>
        /// Try to move a task from the last column.
        /// Should fail.
        /// </summary>
        public bool MoveTask_FromLastColumn()
        {
            string email = "test@example.com";
            string boardName = "Board";
            _factory.Bs.CreateBoard(email, boardName);
            var resp = _factory.Ts.AddTask(email, boardName, "Title", "Desc", DateTime.Now.AddDays(2));
            int taskId = (int)resp.ReturnValue;

            _factory.Ts.MoveTask(email, boardName, 0, taskId);
            _factory.Ts.MoveTask(email, boardName, 1, taskId); // move to last
            var result = _factory.Ts.MoveTask(email, boardName, 2, taskId); // should fail
            return result.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with non-existent board.
        /// </summary>
        public bool MoveTask_InvalidBoard()
        {
            var resp = _factory.Ts.MoveTask("test@example.com", "FakeBoard", 0, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with invalid email.
        /// </summary>
        public bool MoveTask_InvalidEmail()
        {
            string board = "Board";
            _factory.Bs.CreateBoard("test@example.com", board);
            var addResp = _factory.Ts.AddTask("test@example.com", board, "Title", "Desc", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            var resp = _factory.Ts.MoveTask("wrong@example.com", board, 0, taskId);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with invalid task ID.
        /// </summary>
        public bool MoveTask_InvalidTaskId()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);

            var resp = _factory.Ts.MoveTask(email, board, 0, 999);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with invalid column index.
        /// </summary>
        public bool MoveTask_InvalidColumn()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);
            var addResp = _factory.Ts.AddTask(email, board, "Title", "Desc", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            var resp = _factory.Ts.MoveTask(email, board, 99, taskId);
            return resp.ErrorMsg != null;
        }


        /// <summary>
        /// Update task with valid data.
        /// </summary>
        public bool UpdateTask_Valid()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);
            var addResp = _factory.Ts.AddTask(email, board, "Old", "Desc", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            var resp = _factory.Ts.UpdateTask(email, board, "New", "Updated", DateTime.Now.AddDays(3), taskId, 0);
            return resp.ErrorMsg == null;
        }

        /// <summary>
        /// Try update with non-existent board.
        /// </summary>
        public bool UpdateTask_InvalidBoard()
        {
            var resp = _factory.Ts.UpdateTask("test@example.com", "FakeBoard", "T", "D", DateTime.Now.AddDays(2), 0, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with invalid email.
        /// </summary>
        public bool UpdateTask_InvalidEmail()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);
            var addResp = _factory.Ts.AddTask(email, board, "Title", "Desc", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            var resp = _factory.Ts.UpdateTask("wrong@example.com", board, "T", "D", DateTime.Now.AddDays(2), taskId, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with invalid ID.
        /// </summary>
        public bool UpdateTask_InvalidId()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);

            var resp = _factory.Ts.UpdateTask(email, board, "T", "D", DateTime.Now.AddDays(2), 999, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with invalid column index.
        /// </summary>
        public bool UpdateTask_InvalidColumn()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);
            var addResp = _factory.Ts.AddTask(email, board, "Title", "Desc", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            var resp = _factory.Ts.UpdateTask(email, board, "T", "D", DateTime.Now.AddDays(2), taskId, 99);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with empty title.
        /// </summary>
        public bool UpdateTask_EmptyTitle()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);
            var addResp = _factory.Ts.AddTask(email, board, "Title", "Desc", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            var resp = _factory.Ts.UpdateTask(email, board, "", "Desc", DateTime.Now.AddDays(2), taskId, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with too long title (> 50).
        /// </summary>
        public bool UpdateTask_TitleTooLong()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);
            var addResp = _factory.Ts.AddTask(email, board, "T", "D", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            string longTitle = new string('A', 51);
            var resp = _factory.Ts.UpdateTask(email, board, longTitle, "Desc", DateTime.Now.AddDays(2), taskId, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with too long description (> 300).
        /// </summary>
        public bool UpdateTask_DescriptionTooLong()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);
            var addResp = _factory.Ts.AddTask(email, board, "T", "D", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            string longDesc = new string('D', 301);
            var resp = _factory.Ts.UpdateTask(email, board, "Title", longDesc, DateTime.Now.AddDays(2), taskId, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with due date before current date.
        /// </summary>
        public bool UpdateTask_PastDueDate()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.Bs.CreateBoard(email, board);
            var addResp = _factory.Ts.AddTask(email, board, "T", "D", DateTime.Now.AddDays(1));
            int taskId = (int)addResp.ReturnValue;

            var resp = _factory.Ts.UpdateTask(email, board, "Title", "Desc", DateTime.Now.AddDays(-1), taskId, 0);
            return resp.ErrorMsg != null;
        }



        public void RunAll()
        {
            Console.WriteLine("🔹 CreateBoardWithValidValues: " + CreateBoardWithValidValues());
            Console.WriteLine("🔹 CreateBoardWithOccupiedName: " + CreateBoardWithOccupiedName());
            Console.WriteLine("🔹 CreateBoard_CaseInsensitiveName: " + CreateBoard_CaseInsensitiveName());
            Console.WriteLine("🔹 DeleteBoard: " + DeleteBoard());
            Console.WriteLine("🔹 DeleteNonExistentBoard: " + DeleteNonExistentBoard());
            Console.WriteLine("🔹 CreateBoardsSameNameForDifferentUsers: " + CreateBoardsSameNameForDifferentUsers());

            Console.WriteLine("🔹 MoveTask_Valid: " + MoveTask_Valid());
            Console.WriteLine("🔹 MoveTask_FromLastColumn: " + MoveTask_FromLastColumn());
            Console.WriteLine("🔹 MoveTask_InvalidBoard: " + MoveTask_InvalidBoard());
            Console.WriteLine("🔹 MoveTask_InvalidEmail: " + MoveTask_InvalidEmail());
            Console.WriteLine("🔹 MoveTask_InvalidTaskId: " + MoveTask_InvalidTaskId());
            Console.WriteLine("🔹 MoveTask_InvalidColumn: " + MoveTask_InvalidColumn());

            Console.WriteLine("🔹 UpdateTask_Valid: " + UpdateTask_Valid());
            Console.WriteLine("🔹 UpdateTask_InvalidBoard: " + UpdateTask_InvalidBoard());
            Console.WriteLine("🔹 UpdateTask_InvalidEmail: " + UpdateTask_InvalidEmail());
            Console.WriteLine("🔹 UpdateTask_InvalidId: " + UpdateTask_InvalidId());
            Console.WriteLine("🔹 UpdateTask_InvalidColumn: " + UpdateTask_InvalidColumn());
            Console.WriteLine("🔹 UpdateTask_EmptyTitle: " + UpdateTask_EmptyTitle());
            Console.WriteLine("🔹 UpdateTask_TitleTooLong: " + UpdateTask_TitleTooLong());
            Console.WriteLine("🔹 UpdateTask_DescriptionTooLong: " + UpdateTask_DescriptionTooLong());
            Console.WriteLine("🔹 UpdateTask_PastDueDate: " + UpdateTask_PastDueDate());
        }
    }
}