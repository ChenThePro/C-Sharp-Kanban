using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class ColumnBL
    {
        private int limit;
        internal List<TaskBL> tasks;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string name;

        internal ColumnBL(int num)
        {
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
                default:
                    throw new ArgumentException("invalid column num");
            }
            tasks = new List<TaskBL>();
            limit = -1;
        }

        internal void Add(TaskBL task, string email)
        {
            if (limit != -1 && limit == tasks.Count)
                throw new InvalidOperationException("exceeds column's limit");
            Log.Info(email + " task added succesfully");
            tasks.Add(task);
        }

        internal void Delete(TaskBL task, string email)
        {
            tasks.Remove(task);
        }

        internal void UpdateTask(string title, DateTime? due, string description, int id, string email)
        {
            foreach (TaskBL task in tasks)
                if (task.id == id)
                {
                    task.Update(title, due, description, email);
                    return;
                }
            throw new KeyNotFoundException("task doensn't exist");
        }

        internal TaskBL GetTaskByIdAndColumn(int id)
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