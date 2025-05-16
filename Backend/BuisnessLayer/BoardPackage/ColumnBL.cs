using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class ColumnBL
    {
        private int _limit;
        private readonly string _name;
        private readonly List<TaskBL> _tasks;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal ColumnBL(int num)
        {
            switch (num)
            {
                case 0:
                    _name = "backlog";
                    break;
                case 1:
                    _name = "in progress";
                    break;
                case 2:
                    _name = "done";
                    break;
                default:
                    Log.Error("invalid column num");
                    throw new ArgumentException("invalid column num");
            }
            _tasks = new List<TaskBL>();
            _limit = -1;
        }

        internal void Add(TaskBL task, string email)
        {
            if (_limit != -1 && _limit == _tasks.Count)
            {
                Log.Error("exceeds column's limit");
                throw new InvalidOperationException("exceeds column's limit");
            }
            Log.Info(email + " task added succesfully");
            _tasks.Add(task);
        }

        internal void Delete(TaskBL task, string email)
        {
            _tasks.Remove(task);
            Log.Info(email + " task removed succesfully");
        }

        internal void UpdateTask(string title, DateTime? due, string description, int id, string email)
        {
            foreach (TaskBL task in _tasks)
                if (task.Id == id)
                {
                    task.Update(title, due, description, email);
                    return;
                }
            Log.Error("task doensn't exist");
            throw new KeyNotFoundException("task doensn't exist");
        }

        internal TaskBL GetTaskByIdAndColumn(int id)
        {
            foreach (TaskBL task in _tasks)
                if (task.Id == id)
                    return task;
            return null;
        }

        internal void LimitColumn(int limit, string email)
        {
            if (limit != -1 && limit < _tasks.Count)
            {
                Log.Error("limit too low");
                throw new InvalidOperationException("limit too low");
            }
            _limit = limit;
        }

        internal List<TaskBL> GetColumn()
        {
            return _tasks;
        }

        internal int GetColumnLimit()
        {
            return _limit;
        }

        internal string GetColumnName()
        {
            return _name;
        }

        internal void AssignTask(int id, string email)
        {
            TaskBL task = GetTaskByIdAndColumn(id);
            if (task == null)
            {
                Log.Error("task " + id + " for " + email + " doesn't exist in " + _name);
                throw new KeyNotFoundException("task doesn't exist");
            }
            task.AssignTask(email);
        }
    }
}