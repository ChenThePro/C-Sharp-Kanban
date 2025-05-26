using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
{
    public class TaskTests
    {
        private string _userEmail = "user@gmail.com";
        private string _password = "Password1";
        private string _boardName = "board";
        private readonly ServiceFactory _factory = new ServiceFactory();

        public bool TestAddTaskSuccessfully()
        {
            _factory.GetUserService().Register(_userEmail, _password);
            _factory.GetBoardService().CreateBoard(_userEmail, _boardName);
            string json = _factory.GetTaskService().AddTask(_userEmail, _boardName, "Task Title", "Some description", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithUnexistedBoard()
        {
            string json = _factory.GetTaskService().AddTask(_userEmail, "unknownBoard", "Task Title", "Description", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithDueBeforeCreation()
        {
            string json = _factory.GetTaskService().AddTask(_userEmail, _boardName, "Task", "Desc", DateTime.MinValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithWrongEmail()
        {
            string json = _factory.GetTaskService().AddTask("badEmail", _boardName, "Task", "Description", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithNoTitle()
        {
            string json = _factory.GetTaskService().AddTask(_userEmail, _boardName, "", "Description", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskLongTitle()
        {
            string longTitle = new string('A', 51);
            string json = _factory.GetTaskService().AddTask(_userEmail, _boardName, longTitle, "Desc", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskLongDescription()
        {
            string longDesc = new string('B', 301);
            string json = _factory.GetTaskService().AddTask(_userEmail, _boardName, "Title", longDesc, DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_Valid()
        {
            string moveJson = _factory.GetTaskService().AdvanceTask(_userEmail, _boardName, 0, 1);
            return moveJson.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_FromLastColumn()
        {
            _factory.GetTaskService().AdvanceTask(_userEmail, _boardName, 1, 1);
            string finalJson = _factory.GetTaskService().AdvanceTask(_userEmail, _boardName, 2, 1);
            return finalJson.Contains("\"ErrorMessage\"") && !finalJson.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidBoard()
        {
            _factory.GetTaskService().AddTask(_userEmail, _boardName, "Title", "go to movie", DateTime.MaxValue);
            string json = _factory.GetTaskService().AdvanceTask(_userEmail, "kuku", 0, 2);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().AdvanceTask("wrong@example.com", _boardName, 0, 2);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidTaskId()
        {
            string json = _factory.GetTaskService().AdvanceTask(_userEmail, _boardName, 0, 999);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().AdvanceTask(_userEmail, _boardName, 99, 2);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidBoard()
        {
            string json = _factory.GetTaskService().UpdateTask(_userEmail, "FakeBoard", 0, 2, DateTime.MaxValue, "T", "D");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().UpdateTask("wrong@example.com", _boardName, 0, 2, DateTime.MaxValue, "T", "D");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidId()
        {
            string json = _factory.GetTaskService().UpdateTask(_userEmail, _boardName, 0, 999, DateTime.MaxValue, "T", "D");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().UpdateTask(_userEmail, _boardName, 999, 2, DateTime.MaxValue, "T", "D");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_EmptyTitle()
        {
            string json = _factory.GetTaskService().UpdateTask(_userEmail, _boardName, 0, 2, DateTime.MaxValue, "", "Desc");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_TitleTooLong()
        {
            string longTitle = new string('A', 51);
            string json = _factory.GetTaskService().UpdateTask(_userEmail, _boardName, 0, 2, DateTime.MaxValue, longTitle, "Desc");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_DescriptionTooLong()
        {
            string longDesc = new string('D', 301);
            string json = _factory.GetTaskService().UpdateTask(_userEmail, _boardName, 0, 2, DateTime.MaxValue, "Title", longDesc);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_PastDueDate()
        {
            string json = _factory.GetTaskService().UpdateTask(_userEmail, _boardName, 0, 2, DateTime.MinValue, "Title", "Desc");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().AssignTask("wrong@example.com", _boardName, 0, 3, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidBoard()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, "FakeBoard", 0, 3, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 1, 3, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidColumnOutOfRange()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 3, 3, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidTaskId()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 0, -1, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidAssigneeEmail()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 0, 3, "notamember@example.com");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_AssigneeAlreadySet()
        {
            string reassignJson = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 0, 3, "another@example.com");
            return reassignJson.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_Valid()
        {
            string createJson = _factory.GetTaskService().AddTask(_userEmail, _boardName, "Task", "Desc", DateTime.MaxValue);
            if (!createJson.Contains("\"ErrorMessage\":null"))
                return false;
            string assignJson = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 0, 3, _userEmail);
            return assignJson.Contains("\"ErrorMessage\":null");
        }

        public void RunAll()
        {
            Console.WriteLine("🔹 TestAddTaskSuccessfully: " + TestAddTaskSuccessfully());
            Console.WriteLine("🔹 TestAddTaskWithUnexistedBoard: " + TestAddTaskWithUnexistedBoard());
            Console.WriteLine("🔹 TestAddTaskWithDueBeforeCreation: " + TestAddTaskWithDueBeforeCreation());
            Console.WriteLine("🔹 TestAddTaskWithWrongEmail: " + TestAddTaskWithWrongEmail());
            Console.WriteLine("🔹 TestAddTaskWithNoTitle: " + TestAddTaskWithNoTitle());
            Console.WriteLine("🔹 TestAddTaskLongTitle: " + TestAddTaskLongTitle());
            Console.WriteLine("🔹 TestAddTaskLongDescription: " + TestAddTaskLongDescription());
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
            Console.WriteLine("🔹 AssignTask_Valid: " + AssignTask_Valid());
            Console.WriteLine("🔹 AssignTask_AssigneeAlreadySet: " + AssignTask_AssigneeAlreadySet());
            Console.WriteLine("🔹 AssignTask_InvalidEmail: " + AssignTask_InvalidEmail());
            Console.WriteLine("🔹 AssignTask_InvalidBoard: " + AssignTask_InvalidBoard());
            Console.WriteLine("🔹 AssignTask_InvalidColumn: " + AssignTask_InvalidColumn());
            Console.WriteLine("🔹 AssignTask_InvalidColumnOutOfRange: " + AssignTask_InvalidColumnOutOfRange());
            Console.WriteLine("🔹 AssignTask_InvalidTaskId: " + AssignTask_InvalidTaskId());
            Console.WriteLine("🔹 AssignTask_InvalidAssigneeEmail: " + AssignTask_InvalidAssigneeEmail());
        }
    }
}