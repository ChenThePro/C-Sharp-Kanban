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
            string json = _factory.GetTaskService().AddTask(_boardName, "Task Title", DateTime.MaxValue, "Some description", DateTime.Today, 1, _userEmail);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithUnexistedBoard()
        {
            string json = _factory.GetTaskService().AddTask("unknownBoard", "Task Title", DateTime.MaxValue, "Description", DateTime.Today, 2, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithDueBeforeCreation()
        {
            string json = _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MinValue, "Desc", DateTime.MaxValue, 3, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithWrongId()
        {
            string json = _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MaxValue, "Description", DateTime.Today, -5, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithWrongEmail()
        {
            string json = _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MaxValue, "Description", DateTime.Today, 4, "badEmail");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithNoTitle()
        {
            string json = _factory.GetTaskService().AddTask(_boardName, "", DateTime.MaxValue, "Description", DateTime.Today, 5, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskLongTitle()
        {
            string longTitle = new string('A', 51);
            string json = _factory.GetTaskService().AddTask(_boardName, longTitle, DateTime.MaxValue, "Desc", DateTime.Today, 6, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskLongDescription()
        {
            string longDesc = new string('B', 301);
            string json = _factory.GetTaskService().AddTask(_boardName, "Title", DateTime.MaxValue, longDesc, DateTime.Today, 7, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskCreationDate()
        {
            string json = _factory.GetTaskService().AddTask(_boardName, "Task", DateTime.MaxValue, "Description", DateTime.Today, 8, _userEmail);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_Valid()
        {
            string rawJson = _factory.GetTaskService().AddTask(_boardName, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 9, _userEmail);
            if (!rawJson.Contains("\"ErrorMessage\":null"))
                return false;
            string moveJson = _factory.GetTaskService().MoveTask(_boardName, 0, 9, _userEmail);
            return moveJson.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_FromLastColumn()
        {
            string rawJson = _factory.GetTaskService().AddTask(_boardName, "Title", DateTime.MaxValue, "Desc", DateTime.Today, 10, _userEmail);
            if (!rawJson.Contains("\"ErrorMessage\":null"))
                return false;
            _factory.GetTaskService().MoveTask(_boardName, 0, 10, _userEmail);
            _factory.GetTaskService().MoveTask(_boardName, 1, 10, _userEmail);
            string finalJson = _factory.GetTaskService().MoveTask(_boardName, 2, 10, _userEmail);
            return finalJson.Contains("\"ErrorMessage\"") && !finalJson.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidBoard()
        {
            string json = _factory.GetTaskService().MoveTask("kuku", 0, 1, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().MoveTask(_boardName, 0, 1, "wrong@example.com");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidTaskId()
        {
            string json = _factory.GetTaskService().MoveTask(_boardName, 0, 999, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().MoveTask(_boardName, 99, 1, _userEmail);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidBoard()
        {
            string json = _factory.GetTaskService().UpdateTask("FakeBoard", "T", "D", DateTime.MaxValue, 70, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 70, "wrong@example.com", 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidId()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 999, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "T", "D", DateTime.MaxValue, 70, _userEmail, 999);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_EmptyTitle()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "", "Desc", DateTime.MaxValue, 70, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_TitleTooLong()
        {
            string longTitle = new string('A', 51);
            string json = _factory.GetTaskService().UpdateTask(_boardName, longTitle, "Desc", DateTime.MaxValue, 70, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_DescriptionTooLong()
        {
            string longDesc = new string('D', 301);
            string json = _factory.GetTaskService().UpdateTask(_boardName, "Title", longDesc, DateTime.MaxValue, 70, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_PastDueDate()
        {
            string json = _factory.GetTaskService().UpdateTask(_boardName, "Title", "Desc", DateTime.MinValue, 70, _userEmail, 0);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
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