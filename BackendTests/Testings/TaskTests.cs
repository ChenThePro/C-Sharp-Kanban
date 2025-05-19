using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
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
            string json = _factory.GetTaskService().AddTask(_boardName, "Task Title", DateTime.MaxValue, "Some description", DateTime.Today, _userEmail);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithUnexistedBoard()
        {
            string json = _factory.GetTaskService().AddTask("unknownBoard", "Task Title", DateTime.MaxValue, "Description", DateTime.Today, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithDueBeforeCreation()
        {
            string json = _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MinValue, "Desc", DateTime.MaxValue, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithWrongEmail()
        {
            string json = _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MaxValue, "Description", DateTime.Today, "badEmail");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithNoTitle()
        {
            string json = _factory.GetTaskService().AddTask(_boardName, "", DateTime.MaxValue, "Description", DateTime.Today, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskLongTitle()
        {
            string longTitle = new string('A', 51);
            string json = _factory.GetTaskService().AddTask(_boardName, longTitle, DateTime.MaxValue, "Desc", DateTime.Today, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskLongDescription()
        {
            string longDesc = new string('B', 301);
            string json = _factory.GetTaskService().AddTask(_boardName, "Title", DateTime.MaxValue, longDesc, DateTime.Today, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_Valid()
        {
            string moveJson = _factory.GetTaskService().MoveTask(_boardName, 0, 1, _userEmail);
            return moveJson.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_FromLastColumn()
        {
            _factory.GetTaskService().MoveTask(_boardName, 1, 1, _userEmail);
            string finalJson = _factory.GetTaskService().MoveTask(_boardName, 2, 1, _userEmail);
            return finalJson.Contains("\"ErrorMessage\"") && !finalJson.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidBoard()
        {
            _factory.GetTaskService().AddTask(_boardName, "Title", DateTime.MaxValue, "go to movie", DateTime.Today, _userEmail);
            string json = _factory.GetTaskService().MoveTask("kuku", 0, 2, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().MoveTask(_boardName, 0, 2, "wrong@example.com");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidTaskId()
        {
            string json = _factory.GetTaskService().MoveTask(_boardName, 0, 999, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().MoveTask(_boardName, 99, 2, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidBoard()
        {
            string json = _factory.GetTaskService().UpdateTask("FakeBoard", "T", "D", DateTime.MaxValue, 2, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 2, "wrong@example.com", 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidId()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 999, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 2, _userEmail, 999);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_EmptyTitle()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "", "Desc", DateTime.MaxValue, 2, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_TitleTooLong()
        {
            string longTitle = new string('A', 51);
            string json = _factory.GetTaskService().UpdateTask(_boardName, longTitle, "Desc", DateTime.MaxValue, 2, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_DescriptionTooLong()
        {
            string longDesc = new string('D', 301);
            string json = _factory.GetTaskService().UpdateTask(_boardName, "Title", longDesc, DateTime.MaxValue, 2, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_PastDueDate()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "Title", "Desc", DateTime.MinValue, 2, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().AssignTask("wrong@example.com", _boardName, 0, 2, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidBoard()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, "FakeBoard", 0, 2, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 1, 2, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidColumnOutOfRange()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 3, 2, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidTaskId()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 0, -1, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidAssigneeEmail()
        {
            string json = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 0, 2, "notamember@example.com");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_AssigneeAlreadySet()
        {
            string assignJson = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 0, 2, _userEmail);
            if (!assignJson.Contains("\"ErrorMessage\":null"))
                return false;
            string reassignJson = _factory.GetTaskService().AssignTask(_userEmail, _boardName, 0, 2, "another@example.com");
            return reassignJson.Contains("\"ErrorMessage\"") && !reassignJson.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_Valid()
        {
            string createJson = _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MaxValue, "Desc", DateTime.Today, _userEmail);
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
            Console.WriteLine("🔹 AssignTask_ValidAssignment: " + AssignTask_Valid());
            Console.WriteLine("🔹 AssignTask_AlreadyAssigned: " + AssignTask_AssigneeAlreadySet());
            Console.WriteLine("🔹 AssignTask_TaskNotFound: " + AssignTask_InvalidEmail());
            Console.WriteLine("🔹 AssignTask_InvalidUser: " + AssignTask_InvalidBoard());
            Console.WriteLine("🔹 AssignTask_InvalidBoard: " + AssignTask_InvalidBoard());
            Console.WriteLine("🔹 AssignTask_NotInBacklog: " + AssignTask_InvalidColumn());
            Console.WriteLine("🔹 AssignTask_NotInBacklog: " + AssignTask_InvalidColumnOutOfRange());
            Console.WriteLine("🔹 AssignTask_NotInBacklog: " + AssignTask_InvalidTaskId());
            Console.WriteLine("🔹 AssignTask_NotInBacklog: " + AssignTask_InvalidAssigneeEmail());
        }
    }
}