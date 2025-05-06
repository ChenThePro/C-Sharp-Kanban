using System.Text.Json;
using Backend.ServiceLayer;

namespace Backend.BackendTests.Testings
{
    public class TaskTests
    {
        private readonly ServiceFactory _factory = new ServiceFactory();
        private string _userEmail = "user@gmail.com";
        private string _password = "Password1";
        private string _boardName = "board";

        public bool TestAddTaskSuccessfully()
        {
            _factory.GetUserService().Register(_userEmail, _password);
            _factory.GetBoardService().CreateBoard(_boardName, _userEmail);
            string json = _factory.GetTaskService().AddTask(_boardName, "Task Title", DateTime.MaxValue, "Some description", DateTime.Today, 1, _userEmail);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            return response.ErrorMsg == null;
        }

        public bool TestAddTaskWithUnexistedBoard()
        {
            Response response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask("unknownBoard", "Task Title", DateTime.MaxValue, "Description", DateTime.Today, 2, _userEmail))!;
            return response.ErrorMsg != null;
        }

        public bool TestAddTaskWithDueBeforeCreation()
        {
            Response response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MinValue, "Desc", DateTime.MaxValue, 3, _userEmail))!;
            return response.ErrorMsg != null;
        }

        public bool TestAddTaskWithWrongId()
        {
            Response response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MaxValue, "Description", DateTime.Today, -5, _userEmail))!;
            return response.ErrorMsg != null;
        }

        public bool TestAddTaskWithWrongEmail()
        {
            Response response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MaxValue, "Description", DateTime.Today, 4, "badEmail"))!;
            return response.ErrorMsg != null;
        }

        public bool TestAddTaskWithNoTitle()
        {
            Response response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask(_boardName, "", DateTime.MaxValue, "Description", DateTime.Today, 5, _userEmail))!;
            return response.ErrorMsg != null;
        }

        public bool TestAddTaskLongTitle()
        {
            string longTitle = new string('A', 51);
            Response response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask(_boardName, longTitle, DateTime.MaxValue, "Desc", DateTime.Today, 6, _userEmail))!;
            return response.ErrorMsg != null;
        }

        public bool TestAddTaskLongDescription()
        {
            string longDesc = new string('B', 301);
            Response response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask(_boardName, "Title", DateTime.MaxValue, longDesc, DateTime.Today, 7, _userEmail))!;
            return response.ErrorMsg != null;
        }

        public bool TestAddTaskCreationDate()
        {
            Response response = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MaxValue, "Description", DateTime.Today, 8, _userEmail))!;
            return response.ErrorMsg == null;
        }

        public bool MoveTask_Valid()
        {
            string rawJson = _factory.GetTaskService().AddTask(_boardName, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 9, _userEmail);
            Response response = JsonSerializer.Deserialize<Response>(rawJson)!;
            if (response.ErrorMsg != null)
                return false;
            string moveJson = _factory.GetTaskService().MoveTask(_boardName, 0, 9, _userEmail);
            Response moveResp = JsonSerializer.Deserialize<Response>(moveJson)!;
            return moveResp.ErrorMsg == null;
        }

        public bool MoveTask_FromLastColumn()
        {
            string rawJson = _factory.GetTaskService().AddTask(_boardName, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 10, _userEmail);
            Response resp = JsonSerializer.Deserialize<Response>(rawJson)!;
            if (resp.ErrorMsg != null)
                return false;
            _factory.GetTaskService().MoveTask(_boardName, 0, 10, _userEmail);
            _factory.GetTaskService().MoveTask(_boardName, 1, 10, _userEmail);
            string finalJson = _factory.GetTaskService().MoveTask(_boardName, 2, 10, _userEmail);
            Response result = JsonSerializer.Deserialize<Response>(finalJson)!;
            return result.ErrorMsg != null;
        }

        public bool MoveTask_InvalidBoard()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().MoveTask("kuku", 0, 1, _userEmail))!;
            return resp.ErrorMsg != null;
        }

        public bool MoveTask_InvalidEmail()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().MoveTask(_boardName, 0, 1, "wrong@example.com"))!;
            return resp.ErrorMsg != null;
        }

        public bool MoveTask_InvalidTaskId()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().MoveTask(_boardName, 0, 999, _userEmail))!;
            return resp.ErrorMsg != null;
        }

        public bool MoveTask_InvalidColumn()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().MoveTask(_boardName, 99, 1, _userEmail))!;
            return resp.ErrorMsg != null;
        }

        public bool UpdateTask_InvalidBoard()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask("FakeBoard", "T", "D", DateTime.MaxValue, 70, _userEmail, 0))!;
            return resp.ErrorMsg != null;
        }

        public bool UpdateTask_InvalidEmail()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 70, "wrong@example.com", 0))!;
            return resp.ErrorMsg != null;
        }

        public bool UpdateTask_InvalidId()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 999, _userEmail, 0))!;
            return resp.ErrorMsg != null;
        }

        public bool UpdateTask_InvalidColumn()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 70, _userEmail, 999))!;
            return resp.ErrorMsg != null;
        }

        public bool UpdateTask_EmptyTitle()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask(_boardName, "", "Desc", DateTime.MaxValue, 70, _userEmail, 0))!;
            return resp.ErrorMsg != null;
        }

        public bool UpdateTask_TitleTooLong()
        {
            string longTitle = new string('A', 51);
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask(_boardName, longTitle, "Desc", DateTime.MaxValue, 70, _userEmail, 0))!;
            return resp.ErrorMsg != null;
        }

        public bool UpdateTask_DescriptionTooLong()
        {
            string longDesc = new string('D', 301);
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask(_boardName, "Title", longDesc, DateTime.MaxValue, 70, _userEmail, 0))!;
            return resp.ErrorMsg != null;
        }

        public bool UpdateTask_PastDueDate()
        {
            Response resp = JsonSerializer.Deserialize<Response>(
                _factory.GetTaskService().UpdateTask(_boardName, "Title", "Desc", DateTime.MinValue, 70, _userEmail, 0))!;
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