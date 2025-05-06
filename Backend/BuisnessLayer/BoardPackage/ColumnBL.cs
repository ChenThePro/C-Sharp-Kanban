using log4net;

namespace Backend.BuisnessLayer.BoardPackage
{
    internal class ColumnBL
    {
        private int limit = -1;
        internal List<TaskBL> tasks;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string name;

        internal ColumnBL(int num)
        {
            tasks = new List<TaskBL>();
            switch (num)
            {
                case 0:
                    name = "backlog";
                    break;
                case 1:
                    name = "in progress";
                    break;
                case 2:
                    name = "done";
                    break;
            }
        }

        internal void Add(TaskBL newTask, string email)
        {
            if (limit == -1 || limit > tasks.Count)
                tasks.Add(newTask);
            Log.Info("task added succesfully");
            throw new InvalidOperationException("exceeds column's limit");
        }

        internal void Delete(TaskBL task, string email)
        {
            tasks.Remove(task);
        }

        internal void UpdateTask(string title, string due, string description, int id, string email)
        {
            foreach (TaskBL task in tasks)
                if (task.id == id)
                {
                    task.Update(title, due, description, id, email);
                    return;
                }
            throw new KeyNotFoundException("task doensn't exist");
        }

        internal TaskBL? GetTaskByIdAndColumn(int id)
        {
            foreach (TaskBL task in tasks)
                if (task.id == id)
                    return task;
            return null;
        }

        internal void LimitColumn(int limit, string email)
        {
            if (limit < tasks.Count)
                throw new InvalidOperationException("limit too low");
            this.limit = limit;
        }

        internal List<TaskBL> GetColumn()
        {
            return tasks;
        }

        internal int GetColumnLimit()
        {
            return limit;
        }

        internal string GetColumnName()
        {
            return name;
        }
      
    }
}
