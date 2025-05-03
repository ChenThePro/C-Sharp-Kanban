using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BuisnessLayer
{
    internal class ColumnBL
    {
        private int limit = -1;
        internal List<TaskBL> tasks;

        internal ColumnBL()
        {
            tasks = new List<TaskBL>();
        }

        internal void Add(TaskBL newTask, string email)
        {
            if (limit == -1 || limit > tasks.Count)
                tasks.Add(newTask);
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
            if (limit < 0)
                throw new ArgumentOutOfRangeException("limit cannot be negative");
            if (limit < tasks.Count)
                throw new InvalidOperationException("limit too low");
            this.limit = limit;
        }
    }
}
