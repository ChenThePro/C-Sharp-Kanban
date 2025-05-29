using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests.Testings
{
    public class TaskTests
    {
        private ServiceFactory _factory = null!;
        private const string EMAIL1 = "test1@gmail.com";
        private const string EMAIL2 = "test2@gmail.com";
        private const string BOARDNAME1 = "My First Board";
        private const string BOARDNAME2 = "My Second Board";

        public bool TestAddTaskSuccessfully()
        {
            string json = _factory.GetTaskService().AddTask(EMAIL1, BOARDNAME2, "Task Title", "Some description", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithUnexistedBoard()
        {
            string json = _factory.GetTaskService().AddTask(EMAIL1, BOARDNAME1, "Task Title", "Description", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithDueBeforeCreation()
        {
            string json = _factory.GetTaskService().AddTask(EMAIL1, BOARDNAME2, "Task", "Desc", DateTime.MinValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithWrongEmail()
        {
            string json = _factory.GetTaskService().AddTask("badEmail", BOARDNAME1, "Task", "Description", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskWithNoTitle()
        {
            string json = _factory.GetTaskService().AddTask(EMAIL1, BOARDNAME2, "", "Description", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskLongTitle()
        {
            string longTitle = new string('A', 51);
            string json = _factory.GetTaskService().AddTask(EMAIL1, BOARDNAME2, longTitle, "Desc", DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool TestAddTaskLongDescription()
        {
            string longDesc = new string('B', 301);
            string json = _factory.GetTaskService().AddTask(EMAIL1, BOARDNAME2, "Title", longDesc, DateTime.MaxValue);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_Valid()
        {
            _factory.GetTaskService().AssignTask(EMAIL1, BOARDNAME2, 0, 1, EMAIL1);
            string moveJson = _factory.GetTaskService().AdvanceTask(EMAIL1, BOARDNAME2, 0, 1);
            return moveJson.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_FromLastColumn()
        {
            _factory.GetTaskService().AdvanceTask(EMAIL1, BOARDNAME2, 1, 1);
            string finalJson = _factory.GetTaskService().AdvanceTask(EMAIL1, BOARDNAME2, 2, 1);
            return finalJson.Contains("\"ErrorMessage\"") && !finalJson.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidBoard()
        {
            _factory.GetTaskService().AddTask(EMAIL1, BOARDNAME2, "Title", "go to movie", DateTime.MaxValue);
            string json = _factory.GetTaskService().AdvanceTask(EMAIL1, BOARDNAME1, 0, 2);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().AdvanceTask("wrong@example.com", BOARDNAME2, 0, 2);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidTaskId()
        {
            string json = _factory.GetTaskService().AdvanceTask(EMAIL1, BOARDNAME2, 0, 999);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool MoveTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().AdvanceTask(EMAIL1, BOARDNAME2, 99, 2);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidBoard()
        {
            string json = _factory.GetTaskService().UpdateTask(EMAIL1, "FakeBoard", 0, 2, DateTime.MaxValue, "T", "D");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().UpdateTask("wrong@example.com", BOARDNAME2, 0, 2, DateTime.MaxValue, "T", "D");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidId()
        {
            string json = _factory.GetTaskService().UpdateTask(EMAIL1, BOARDNAME2, 0, 999, DateTime.MaxValue, "T", "D");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().UpdateTask(EMAIL1, BOARDNAME2, 999, 2, DateTime.MaxValue, "T", "D");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_EmptyTitle()
        {
            string json = _factory.GetTaskService().UpdateTask(EMAIL1, BOARDNAME2, 0, 2, DateTime.MaxValue, "", "Desc");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_TitleTooLong()
        {
            string longTitle = new string('A', 51);
            string json = _factory.GetTaskService().UpdateTask(EMAIL1, BOARDNAME2, 0, 2, DateTime.MaxValue, longTitle, "Desc");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_DescriptionTooLong()
        {
            string longDesc = new string('D', 301);
            string json = _factory.GetTaskService().UpdateTask(EMAIL1, BOARDNAME2, 0, 2, DateTime.MaxValue, "Title", longDesc);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool UpdateTask_PastDueDate()
        {
            string json = _factory.GetTaskService().UpdateTask(EMAIL1, BOARDNAME2, 0, 2, DateTime.MinValue, "Title", "Desc");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidEmail()
        {
            string json = _factory.GetTaskService().AssignTask("wrong@example.com", BOARDNAME2, 0, 2, EMAIL1);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidBoard()
        {
            string json = _factory.GetTaskService().AssignTask(EMAIL2, "FakeBoard", 0, 2, EMAIL1);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidColumn()
        {
            string json = _factory.GetTaskService().AssignTask(EMAIL2, BOARDNAME2, 1, 2, EMAIL1);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidColumnOutOfRange()
        {
            string json = _factory.GetTaskService().AssignTask(EMAIL2, BOARDNAME2, 3, 2, EMAIL1);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidTaskId()
        {
            string json = _factory.GetTaskService().AssignTask(EMAIL2, BOARDNAME2, 0, -1, EMAIL1);
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_InvalidAssigneeEmail()
        {
            string json = _factory.GetTaskService().AssignTask(EMAIL2, BOARDNAME2, 0, 2, "empty@gmail.com");
            return json.Contains("\"ErrorMessage\"") && !json.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_Valid()
        {
            string assignJson = _factory.GetTaskService().AssignTask(EMAIL2, BOARDNAME2, 0, 2, EMAIL1);
            return assignJson.Contains("\"ErrorMessage\":null");
        }

        public bool AssignTask_AssigneeAlreadySet()
        {
            _factory.GetBoardService().CreateBoard(EMAIL1, "kuku");
            _factory.GetTaskService().AddTask(EMAIL1, "kuku", "t", "d", DateTime.MaxValue);
            string reassignJson = _factory.GetTaskService().AssignTask(EMAIL2, BOARDNAME2, 0, 2, EMAIL2);
            return reassignJson.Contains("\"ErrorMessage\"") && !reassignJson.Contains("\"ErrorMessage\":null");
        }

        public void RunAll(ServiceFactory serviceFactory)
        {
            _factory = serviceFactory;
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