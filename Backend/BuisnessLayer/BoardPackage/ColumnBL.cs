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
                _ => throw new ArgumentOutOfRangeException(nameof(num), "Invalid column index")
            };
            _limit = limit;
            _tasks = tasks;
        }

        internal void AddTask(string email, TaskBL task)
        {
            if (_limit != -1 && _tasks.Count >= _limit)
            {
                Log.Error("Adding task exceeds column limit.");
                throw new InvalidOperationException("Adding task exceeds column limit.");
            }
            _tasks.Add(task);
            Log.Info($"Task added successfully by {email} to '{_name}' column.");
        }

        internal void DeleteTask(string email, TaskBL task)
        {
            _tasks.Remove(task);
            Log.Info($"Task removed successfully by {email} from '{_name}' column.");
        }

        internal void UpdateTask(string email, int taskID, DateTime? dueDate, string title, string description)
        {
            TaskBL task = GetTaskById(taskID);
            TaskExists(task, taskID);
            task.Update(email, dueDate, title, description);
        }

        internal TaskBL GetTaskById(int taskID) => 
            _tasks.Find(task => task.Id == taskID);

        internal void Limit(string email, int limit)
        {
            if (limit != -1 && limit < _tasks.Count)
            {
                Log.Error($"New limit {limit} is less than current task count {_tasks.Count}.");
                throw new InvalidOperationException($"New limit {limit} is less than current task count {_tasks.Count}.");
            }
            _limit = limit;
            Log.Info($"Column '{_name}' limit set to {limit} by {email}.");
        }

        internal List<TaskBL> GetTasks() => _tasks;

        internal int GetLimit() => _limit;

        internal string GetName() => _name;

        internal void AssignTask(string email, int taskID, string emailAssignee)
        {
            TaskBL task = GetTaskById(taskID);
            TaskExists(task, taskID);
            task.Assign(email, emailAssignee);
        }

        internal void TaskExists(TaskBL task, int taskID)
        {
            if (task == null)
            {
                Log.Error($"Task ID {taskID} not found in column '{_name}'.");
                throw new KeyNotFoundException($"Task ID {taskID} not found in column '{_name}'.");
            }
        }
    }
}