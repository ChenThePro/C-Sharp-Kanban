using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.ServiceLayer;

namespace Tests
{
    public class TaskTests
    {
        private ServiceFactory _factory;

        /// <summary>
        /// Test adding a task successfully.
        /// Preconditions: Valid board name, title, description, due date, ID, and email.
        /// Postconditions: Task is added without error.
        /// Throws: None.
        /// </summary>
        public bool TestAddTaskSuccessfully()
        {
            Response response = _factory.Ts.AddTask("board1", "Task Title", "Some description", DateTime.Now.AddDays(1), 1, "user@email.com");
            if (response.ErrorMsg != null || response.RetVal == null)
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
            Response response = _factory.Ts.AddTask("unknownBoard", "Task Title", "Description", DateTime.Now.AddDays(1), 2, "user@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Ts.AddTask("board1", "Task", "Desc", DateTime.Now.AddDays(-1), 3, "user@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Ts.AddTask("board1", "Task", "Description", DateTime.Now.AddDays(1), -5, "user@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Ts.AddTask("board1", "Task", "Description", DateTime.Now.AddDays(1), 4, "badEmail");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Ts.AddTask("board1", "", "Description", DateTime.Now.AddDays(1), 5, "user@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Ts.AddTask("board1", longTitle, "Desc", DateTime.Now.AddDays(1), 6, "user@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            Response response = _factory.Ts.AddTask("board1", "Title", longDesc, DateTime.Now.AddDays(1), 7, "user@email.com");
            if (response.ErrorMsg == null || response.RetVal != null)
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
            _factory.Ts.AddTask("board1", "Task", "Description", DateTime.Now.AddDays(1), 8, "user@email.com");
            TaskSL task = _factory.Ts.GetTaskById(8);
            if (task.CreationTime == null)
                return false;
            return true;
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
        }
    }
}