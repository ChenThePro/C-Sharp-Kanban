using System.Text.Json;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class TaskTests
    {
        private readonly ServiceFactory _factory = new ServiceFactory();

        public bool TestAddTaskSuccessfully()
        {
            _factory.GetUserService().Register("user@gmail.com", "Password1");
            _factory.GetBoardService().CreateBoard("board", "user@gmail.com");
            var json = _factory.GetTaskService().AddTask("board", "Task Title", DateTime.MaxValue, "Some description", DateTime.Today, 1, "user@gmail.com");
            var response = JsonSerializer.Deserialize<Response>(json);
            return response?.ErrorMsg == null;
        }

        public bool TestAddTaskWithUnexistedBoard()
        {
            var response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("unknownBoard", "Task Title", DateTime.MaxValue, "Description", DateTime.Today, 2, "user@gmail.com"));
            return response?.ErrorMsg != null;
        }

        public bool TestAddTaskWithDueBeforeCreation()
        {
            var response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("board", "Task", DateTime.MinValue, "Desc", DateTime.MaxValue, 3, "user@gmail.com"));
            return response?.ErrorMsg != null;
        }

        public bool TestAddTaskWithWrongId()
        {
            var response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("board", "Task", DateTime.MaxValue, "Description", DateTime.Today, -5, "user@gmail.com"));
            return response?.ErrorMsg != null;
        }

        public bool TestAddTaskWithWrongEmail()
        {
            var response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("board", "Task", DateTime.MaxValue, "Description", DateTime.Today, 4, "badEmail"));
            return response?.ErrorMsg != null;
        }

        public bool TestAddTaskWithNoTitle()
        {
            var response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("board", "", DateTime.MaxValue, "Description", DateTime.Today, 5, "user@gmail.com"));
            return response?.ErrorMsg != null;
        }

        public bool TestAddTaskLongTitle()
        {
            string longTitle = new string('A', 51);
            var response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("board", longTitle, DateTime.MaxValue, "Desc", DateTime.Today, 6, "user@gmail.com"));
            return response?.ErrorMsg != null;
        }

        public bool TestAddTaskLongDescription()
        {
            string longDesc = new string('B', 301);
            var response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("board", "Title", DateTime.MaxValue, longDesc, DateTime.Today, 7, "user@gmail.com"));
            return response?.ErrorMsg != null;
        }

        public bool TestAddTaskCreationDate()
        {
            var response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("board", "Task", DateTime.MaxValue, "Description", DateTime.Today, 8, "user@gmail.com"));
            return response?.ErrorMsg == null;
        }

        public bool MoveTask_Valid()
        {
            string email = "user@gmail.com";
            string boardName = "board";
            string rawJson = _factory.GetTaskService().AddTask(boardName, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 9, email);
            Response? response = JsonSerializer.Deserialize<Response>(rawJson);
            if (response == null || response.ErrorMsg != null)
                return false;
            if (response.RetVal is not JsonElement jsonElement)
                return false;
            TaskSL? task = jsonElement.Deserialize<TaskSL>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (task == null)
                return false;
            int taskId = task.Id;
            string moveJson = _factory.GetTaskService().MoveTask(boardName, 0, taskId, email);
            Response? moveResp = JsonSerializer.Deserialize<Response>(moveJson);
            return moveResp != null && moveResp.ErrorMsg == null;
        }


        public bool MoveTask_FromLastColumn()
        {
            string email = "user@gmail.com";
            string boardName = "Board";
            string rawJson = _factory.GetTaskService().AddTask(boardName, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 10, email);
            Response? resp = JsonSerializer.Deserialize<Response>(rawJson);
            if (resp == null || resp.RetVal is not JsonElement jsonElement)
                return false;
            TaskSL? task = jsonElement.Deserialize<TaskSL>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (task == null)
                return false;
            int taskId = task.Id;
            _factory.GetTaskService().MoveTask(boardName, 0, taskId, email);
            _factory.GetTaskService().MoveTask(boardName, 1, taskId, email);
            string finalJson = _factory.GetTaskService().MoveTask(boardName, 2, taskId, email);
            Response? result = JsonSerializer.Deserialize<Response>(finalJson);
            return result != null && result.ErrorMsg != null;
        }



        public bool MoveTask_InvalidBoard()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().MoveTask("Board", 0, 1, "test@example.com"));
            return resp?.ErrorMsg != null;
        }

        public bool MoveTask_InvalidEmail()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().MoveTask("board", 0, 1, "wrong@example.com"));
            return resp?.ErrorMsg != null;
        }

        public bool MoveTask_InvalidTaskId()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().MoveTask("Board", 0, 999, "user@gmail.com"));
            return resp?.ErrorMsg != null;
        }

        public bool MoveTask_InvalidColumn()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().MoveTask("board", 99, 1, "user@gmail.com"));
            return resp?.ErrorMsg != null;
        }

        public bool UpdateTask_InvalidBoard()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("FakeBoard", "T", "D", DateTime.MaxValue, 70, "user@gmail.com", 0));
            return resp?.ErrorMsg != null;
        }

        public bool UpdateTask_InvalidEmail()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("Board", "T", "D", DateTime.MaxValue, 70, "wrong@example.com", 0));
            return resp?.ErrorMsg != null;
        }

        public bool UpdateTask_InvalidId()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("board", "T", "D", DateTime.MaxValue, 999, "user@gmail.com", 0));
            return resp?.ErrorMsg != null;
        }

        public bool UpdateTask_InvalidColumn()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("board", "T", "D", DateTime.MaxValue, 70, "user@gmail.com", 999));
            return resp?.ErrorMsg != null;
        }

        public bool UpdateTask_EmptyTitle()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("board", "", "Desc", DateTime.MaxValue, 70, "user@gmail.com", 0));
            return resp?.ErrorMsg != null;
        }

        public bool UpdateTask_TitleTooLong()
        {
            string longTitle = new string('A', 51);
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("board", longTitle, "Desc", DateTime.MaxValue, 70, "user@gmail.com", 0));
            return resp?.ErrorMsg != null;
        }

        public bool UpdateTask_DescriptionTooLong()
        {
            string longDesc = new string('D', 301);
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("board", "Title", longDesc, DateTime.MaxValue, 70, "user@gmail.com", 0));
            return resp?.ErrorMsg != null;
        }

        public bool UpdateTask_PastDueDate()
        {
            var resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("board", "Title", "Desc", DateTime.MinValue, 70, "user@gmail.com", 0));
            return resp?.ErrorMsg != null;
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
