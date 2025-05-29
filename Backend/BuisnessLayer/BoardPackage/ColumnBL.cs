using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class ColumnBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _name;
        private int _limit;
        private readonly List<TaskBL> _tasks;

        internal ColumnBL(int num, int limit, List<TaskBL> tasks)
        {
            _name = num switch
            {
                0 => "backlog",
                1 => "in progress",
                2 => "done",
                _ => throw new NotImplementedException("Will never reach here"),
            };
            _limit = limit;
            _tasks = tasks;
        }

        internal void Add(string email, TaskBL task)
        {
            if (_limit != -1 && _limit == _tasks.Count)
            {
                Log.Error("Add a task will exceed column's limit.");
                throw new InvalidOperationException("Add a task will exceed column's limit.");
            }
            Log.Info("Task added succesfully for " + email + ".");
            _tasks.Add(task);
        }

        internal void Delete(string email, TaskBL task)
        {
            _tasks.Remove(task);
            Log.Info("Task removed succesfully " + email + ".");
        }

        internal void UpdateTask(string email, int taskID, DateTime? dueDate, string title, string description)
        {
            TaskBL task = GetTaskById(taskID);
            if (task == null)
            {
                Log.Error("Task doensn't exist.");
                throw new KeyNotFoundException("Task doensn't exist.");
            }
            task.Update(email, dueDate, title, description);
        }

        internal TaskBL GetTaskById(int taskID)
        {
            foreach (TaskBL task in _tasks)
                if (task.Id == taskID)
                    return task;
            return null;
        }

        internal void LimitColumn(string email, int limit)
        {
            if (limit != -1 && limit < _tasks.Count)
            {
                Log.Error("Limit lower than current tasks in " + _name + ".");
                throw new InvalidOperationException("Limit lower than current tasks in " + _name + ".");
            }
            _limit = limit;
        }

        internal List<TaskBL> GetTasks()
        {
            return _tasks;
        }

        internal int GetLimit()
        {
            return _limit;
        }

        internal string GetName()
        {
            return _name;
        }

        internal void AssignTask(string email, int taskID, string emailAssignee)
        {
            TaskBL task = GetTaskById(taskID);
            if (task == null)
            {
                Log.Error("Task id " + taskID + " for " + emailAssignee + " doesn't exist in " + _name);
                throw new KeyNotFoundException("Task id " + taskID + " for " + emailAssignee + " doesn't exist in " + _name);
            }
            task.AssignTask(email, emailAssignee);
        }
    }
}