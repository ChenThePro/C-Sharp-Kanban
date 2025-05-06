using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class TaskTests
    {
        private ServiceFactory _factory = new ServiceFactory();

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
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("board", "Task Title", DateTime.MaxValue, "Some description", DateTime.Today, 1, "user@email.com"));
            return response.ErrorMsg == null;
        }

        /// <summary>
        /// Test adding a task to a non-existent board.
        /// Preconditions: Board does not exist.
        /// Postconditions: Task addition fails with error.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithUnexistedBoard()
        {
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("unknownBoard", "Task Title", DateTime.MaxValue, "Description", DateTime.Today, 2, "user@email.com"));
            return response.ErrorMsg != null;
        }

        /// <summary>
        /// Test adding a task with a due date before creation date.
        /// Preconditions: Due date is before the current date.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithDueBeforeCreation()
        {
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("board1", "Task", DateTime.MinValue, "Desc", DateTime.MaxValue, 3, "user@email.com"));
            return response.ErrorMsg != null;
        }

        /// <summary>
        /// Test adding a task with an invalid task ID.
        /// Preconditions: Task ID is invalid.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithWrongId()
        {
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("board", "Task", DateTime.MaxValue, "Description", DateTime.Today, -5, "user@email.com"));
            return response.ErrorMsg != null;
        }

        /// <summary>
        /// Test adding a task with an invalid email.
        /// Preconditions: Email is in incorrect format or not recognized.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithWrongEmail()
        {
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("board", "Task", DateTime.MaxValue, "Description", DateTime.Today, 4, "badEmail"));
            return response.ErrorMsg != null;
        }

        /// <summary>
        /// Test adding a task with no title.
        /// Preconditions: Title is an empty string.
        /// Postconditions: Task addition fails.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskWithNoTitle()
        {
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("board", "", DateTime.MaxValue, "Description", DateTime.Today, 5, "user@email.com"));
            return response.ErrorMsg != null;
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
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("board1", longTitle, DateTime.MaxValue, "Desc", DateTime.Today, 6, "user@email.com"));
            return response.ErrorMsg != null;
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
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("board1", "Title", DateTime.MaxValue, longDesc, DateTime.Today, 7, "user@email.com"));
            return response.ErrorMsg != null;
        }

        /// <summary>
        /// Test that task creation time is set correctly.
        /// Preconditions: Task is added successfully.
        /// Postconditions: Creation time is a valid non-null value.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskCreationDate()
        {
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask("board1", "Task", DateTime.MaxValue, "Description", DateTime.Today, 8, "user@email.com"));
            return response.ErrorMsg == null;
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
            Response<TaskSL> response = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(boardName, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 9, email));
            if (response.ErrorMsg != null) return false;
            int taskId = response.RetVal.Id;
            Response<object> moveResp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().MoveTask(boardName, 0, taskId, email));
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
            Response<TaskSL> resp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(boardName, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 9, email));
            int taskId = resp.RetVal.Id;
            _factory.GetTaskService().MoveTask(boardName, 0, taskId, email);
            _factory.GetTaskService().MoveTask(boardName, 1, taskId, email);
            Response<object> result = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().MoveTask(boardName, 2, taskId, email));
            return result.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with non-existent board.
        /// </summary>
        public bool MoveTask_InvalidBoard()
        {
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().MoveTask("FakeBoard", 0, 0, "test@example.com"));
            return resp.ErrorMsg != null;
        }

        /// <summary>
        /// Try to move a task with invalid email.
        /// </summary>
        public bool MoveTask_InvalidEmail()
        {
            string board = "Board";
            JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetBoardService().CreateBoard("test@example.com", board));
            Response<TaskSL> addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 10, "test@example.com"));
            int taskId = addResp.RetVal.Id;
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().MoveTask(board, 0, taskId, "wrong@example.com"));
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
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().MoveTask(board, 0, 999, email));
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
            Response<TaskSL> addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 11, email));
            int taskId = addResp.RetVal.Id;
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().MoveTask(board, 99, taskId, email));
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
            Response<TaskSL> addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "Old", DateTime.MaxValue, "Desc", DateTime.Today, 12, email));
            int taskId = addResp.RetVal.Id;
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask(board, "New", "Updated", DateTime.MaxValue, taskId, email, 0));
            return resp.ErrorMsg == null;
        }

        /// <summary>
        /// Try update with non-existent board.
        /// </summary>
        public bool UpdateTask_InvalidBoard()
        {
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask("FakeBoard", "T", "D", DateTime.MaxValue, 0, "test@example.com", 0));
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
            Response<TaskSL> addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 20, email));
            int taskId = addResp.RetVal.Id;
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask(board, "T", "D", DateTime.MaxValue, taskId, "wrong@example.com", 0));
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
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask(board, "T", "D", DateTime.MaxValue, 999, email, 0));
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
            Response<TaskSL> addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 21, email));
            int taskId = addResp.RetVal.Id;
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask(board, "T", "D", DateTime.MaxValue, taskId, email, 0));
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
            Response<TaskSL> addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 22, email));
            int taskId = addResp.RetVal.Id;
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask(board, "", "Desc", DateTime.MaxValue, taskId, email, 0));
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
            Response<TaskSL> addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "T", DateTime.MaxValue, "D", DateTime.Today, 23, email));
            int taskId = addResp.RetVal.Id;
            string longTitle = new string('A', 51);
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask(board, longTitle, "Desc", DateTime.MaxValue, taskId, email, 0));
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
            Response<TaskSL> addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "T", DateTime.MaxValue, "D", DateTime.Today, 24, email));
            int taskId = addResp.RetVal.Id;
            string longDesc = new string('D', 301);
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask(board, "Title", longDesc, DateTime.MaxValue, taskId, email, 0));
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
            var addResp = JsonSerializer.Deserialize<Response<TaskSL>>(_factory.GetTaskService().AddTask(board, "T", DateTime.MaxValue, "D", DateTime.Today, 25, email));
            int taskId = addResp.RetVal.Id;
            Response<object> resp = JsonSerializer.Deserialize<Response<object>>(_factory.GetTaskService().UpdateTask(board, "Title", "Desc", DateTime.MaxValue, taskId, email, 0));
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
