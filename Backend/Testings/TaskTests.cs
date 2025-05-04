using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class TaskTests
    {
        private ServiceFactory _factory = new ServiceFactory(new BoardFacade(), new UserFacade());

        /// <summary>
        /// Test adding a task successfully.
        /// Preconditions: Valid board name, title, description, due date, ID, and email.
        /// Postconditions: Task is added without error.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskSuccessfully()
        {
            _factory.GetUserService().Register("user@gmail.com", "Password1");
            _factory.GetBoardService().CreateBoard("board", "user@gmail.com");
            Response<TaskSL> response = _factory.GetTaskService().AddTask("board", "Task Title", "19/09/9999", "Some description", DateTime.Today.ToString(), 1, "user@email.com");
            if (response.RetVal == null)
                return false;
            return true;
        }

        /// <summary>
        /// Test adding a task to a non-existent board.
        /// Preconditions: Board does not exist.
        /// Postconditions: Task addition fails with error.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithUnexistedBoard()
        {
            Response<TaskSL> response = _factory.GetTaskService().AddTask("unknownBoard", "Task Title", "19/09/9999", "Description", DateTime.Today.ToString(), 2, "user@email.com");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test adding a task with a due date before creation date.
        /// Preconditions: Due date is before the current date.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithDueBeforeCreation()
        {
            Response<TaskSL> response = _factory.GetTaskService().AddTask("board1", "Task", "19/09/9999", "Desc", "20/09/9999", 3, "user@email.com");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test adding a task with an invalid task ID.
        /// Preconditions: Task ID is invalid.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithWrongId()
        {
            Response<TaskSL> response = _factory.GetTaskService().AddTask("board", "Task", "19/09/9999", "Description", DateTime.Today.ToString(), -5, "user@email.com");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test adding a task with an invalid email.
        /// Preconditions: Email is in incorrect format or not recognized.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithWrongEmail()
        {
            Response<TaskSL> response = _factory.GetTaskService().AddTask("board", "Task", "19/09/9999", "Description", DateTime.Today.ToString(), 4, "badEmail");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test adding a task with no title.
        /// Preconditions: Title is an empty string.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithNoTitle()
        {
            Response<TaskSL> response = _factory.GetTaskService().AddTask("board", "", "19/09/9999", "Description", DateTime.Today.ToString(), 5, "user@email.com");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test adding a task with an excessively long title.
        /// Preconditions: Title exceeds allowed length.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskLongTitle()
        {
            string longTitle = new string('A', 51);
            Response<TaskSL> response = _factory.GetTaskService().AddTask("board1", longTitle, "19/09/9999", "Desc", DateTime.Today.ToString(), 6, "user@email.com");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test adding a task with an excessively long description.
        /// Preconditions: Description exceeds allowed length.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskLongDescription()
        {
            string longDesc = new string('B', 301);
            Response<TaskSL> response = _factory.GetTaskService().AddTask("board1", "Title", "19/09/9999", longDesc, DateTime.Today.ToString(), 7, "user@email.com");
            if (response.RetVal != null)
                return false;
            return true;
        }

        /// <summary>
        /// Test that task creation time is set correctly.
        /// Preconditions: Task is added successfully.
        /// Postconditions: Creation time is a valid non-null value.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskCreationDate()
        {
            Response<TaskSL> response = _factory.GetTaskService().AddTask("board1", "Task", "19/09/9999", "Description", DateTime.Today.ToString(), 8, "user@email.com");
            if (response.RetVal.CreationTime == null)
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
            string email = "user@example.com";
            string boardName = "board";
            Response<TaskSL> response = _factory.GetTaskService().AddTask(boardName, "Title", "19/09/9999", "Desc", DateTime.Today.ToString(), 9, email);
            if (response.RetVal == null) return false;
            int taskId = response.RetVal.Id;
            var moveResp = _factory.GetTaskService().MoveTask(boardName, 0, taskId, email);
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
            Response<TaskSL> resp = _factory.GetTaskService().AddTask(boardName, "Title", "19/09/9999", "Desc", DateTime.Today.ToString(), 9, email);
            int taskId = resp.RetVal.Id;

            _factory.GetTaskService().MoveTask(boardName, 0, taskId, email);
            _factory.GetTaskService().MoveTask(boardName, 1, taskId, email);
            var result = _factory.GetTaskService().MoveTask(boardName, 2, taskId, email);
            return result.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with non-existent board.
        /// </summary>
        public bool MoveTask_InvalidBoard()
        {
            var resp = _factory.GetTaskService().MoveTask("FakeBoard", 0, 0, "test@example.com");
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with invalid email.
        /// </summary>
        public bool MoveTask_InvalidEmail()
        {
            string board = "Board";
            _factory.GetBoardService().CreateBoard("test@example.com", board);
            var addResp = _factory.GetTaskService().AddTask(board, "Title", "19/09/9999", "Desc", DateTime.Today.ToString(), 10, "test@example.com");
            int taskId = addResp.RetVal.Id;

            var resp = _factory.GetTaskService().MoveTask(board, 0, taskId, "wrong@example.com");
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with invalid task ID.
        /// </summary>
        public bool MoveTask_InvalidTaskId()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);

            var resp = _factory.GetTaskService().MoveTask(board, 0, 999, email);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with invalid column index.
        /// </summary>
        public bool MoveTask_InvalidColumn()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);
            var addResp = _factory.GetTaskService().AddTask(board, "Title", "19/09/9999", "Desc", DateTime.Today.ToString(), 11, email);
            int taskId = addResp.RetVal.Id;

            var resp = _factory.GetTaskService().MoveTask(board, 99, taskId, email);
            return resp.ErrorMsg != null;
        }


        /// <summary>
        /// Update task with valid data.
        /// </summary>
        public bool UpdateTask_Valid()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);
            var addResp = _factory.GetTaskService().AddTask(board, "Old", "19/09/9999", "Desc", DateTime.Today.ToString(), 12, email);
            int taskId = addResp.RetVal.Id;

            var resp = _factory.GetTaskService().UpdateTask(board, "New", "Updated", "20/09/9999", taskId, email, 0);
            return resp.ErrorMsg == null;
        }

        /// <summary>
        /// Try update with non-existent board.
        /// </summary>
        public bool UpdateTask_InvalidBoard()
        {
            var resp = _factory.GetTaskService().UpdateTask("FakeBoard", "T", "D", "19/09/9999", 0, "test@example.com", 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with invalid email.
        /// </summary>
        public bool UpdateTask_InvalidEmail()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);
            var addResp = _factory.GetTaskService().AddTask(board, "Title", "19/09/9999", "Desc", DateTime.Today.ToString(), 20, email);
            int taskId = addResp.RetVal.Id;

            var resp = _factory.GetTaskService().UpdateTask(board, "T", "D", "20/09/9999", taskId, "wrong@example.com", 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with invalid ID.
        /// </summary>
        public bool UpdateTask_InvalidId()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);

            var resp = _factory.GetTaskService().UpdateTask(board, "T", "D", "19/09/9999", 999, email, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with invalid column index.
        /// </summary>
        public bool UpdateTask_InvalidColumn()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);
            var addResp = _factory.GetTaskService().AddTask(board, "Title", "19/09/9999", "Desc", DateTime.Today.ToString(), 21, email);
            int taskId = addResp.RetVal.Id;

            var resp = _factory.GetTaskService().UpdateTask(board, "T", "D", "20/09/9999", taskId, email, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with empty title.
        /// </summary>
        public bool UpdateTask_EmptyTitle()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);
            var addResp = _factory.GetTaskService().AddTask(board, "Title", "19/09/9999", "Desc", DateTime.Today.ToString(), 22, email);
            int taskId = addResp.RetVal.Id;

            var resp = _factory.GetTaskService().UpdateTask(board, "", "Desc", "20/09/9999", taskId, email, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with too long title (> 50).
        /// </summary>
        public bool UpdateTask_TitleTooLong()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);
            var addResp = _factory.GetTaskService().AddTask(board, "T", "19/09/9999", "D", DateTime.Today.ToString(), 23, email);
            int taskId = addResp.RetVal.Id;

            string longTitle = new string('A', 51);
            var resp = _factory.GetTaskService().UpdateTask(board, longTitle, "Desc", "20/09/9999", taskId, email, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with too long description (> 300).
        /// </summary>
        public bool UpdateTask_DescriptionTooLong()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);
            var addResp = _factory.GetTaskService().AddTask(board, "T", "19/09/9999", "D", DateTime.Today.ToString(), 24, email);
            int taskId = addResp.RetVal.Id;

            string longDesc = new string('D', 301);
            var resp = _factory.GetTaskService().UpdateTask(board, "Title", longDesc, "20/09/9999", taskId, email, 0);
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try update with due date before current date.
        /// </summary>
        public bool UpdateTask_PastDueDate()
        {
            string email = "test@example.com";
            string board = "Board";
            _factory.GetBoardService().CreateBoard(email, board);
            var addResp = _factory.GetTaskService().AddTask(board, "T", "19/09/9999", "D", DateTime.Today.ToString(), 25, email);
            int taskId = addResp.RetVal.Id;

            var resp = _factory.GetTaskService().UpdateTask(board, "Title", "Desc", "19/09/9999", taskId, email, 0);
            return resp.ErrorMsg != null;
        }

        public void RunAll()
        {
            Console.WriteLine("🔹 TestAddTaskSuccessfully: " + TestAddTaskSuccessfully());
            Console.WriteLine("🔹 TestAddTaskWithUnexistedBoard: " + TestAddTaskWithUnexistedBoard());
            Console.WriteLine("🔹 TestAddTaskWithDueBeforeCreation: " + TestAddTaskWithDueBeforeCreation());
            Console.WriteLine("🔹 TestAddTaskWithWrongId: " + TestAddTaskWithWrongId());
            Console.WriteLine("🔹 TestAddTaskWithWrongEmail: " + TestAddTaskWithWrongEmail());
            Console.WriteLine("🔹 TestAddTaskWithNoTitle: " + TestAddTaskWithNoTitle());
            Console.WriteLine("🔹 TestAddTaskLongTitle: " + TestAddTaskLongTitle());
            Console.WriteLine("🔹 TestAddTaskLongDescription: " + TestAddTaskLongDescription());
            Console.WriteLine("🔹 TestAddTaskCreationDate: " + TestAddTaskCreationDate());
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