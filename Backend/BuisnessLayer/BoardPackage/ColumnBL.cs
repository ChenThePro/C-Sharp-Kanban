using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class ColumnBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal readonly string Name;
        internal int Limit;
        internal readonly List<TaskBL> Tasks;

        internal ColumnBL(int num, int limit, List<TaskBL> tasks)
        {
            Name = num switch
            {
                0 => "backlog",
                1 => "in progress",
                2 => "done",
                _ => throw new ArgumentOutOfRangeException(nameof(num), "Invalid column index")
            };
            Limit = limit;
            Tasks = tasks;
        }

        internal void AddTask(string email, TaskBL task)
        {
            if (Limit != -1 && Tasks.Count >= Limit)
            {
                Log.Error("Adding task exceeds column limit.");
                throw new InvalidOperationException("Adding task exceeds column limit.");
            }
            Tasks.Add(task);
            Log.Info($"Task added successfully by {email} to '{Name}' column.");
        }

        internal void DeleteTask(string email, TaskBL task)
        {
            Tasks.Remove(task);
            Log.Info($"Task removed successfully by {email} from '{Name}' column.");
        }

        internal void UpdateTask(string email, int taskID, DateTime? dueDate, string title, string description)
        {
            TaskBL task = GetTaskById(taskID);
            TaskExists(task, taskID);
            task.Update(email, dueDate, title, description);
        }

        internal TaskBL GetTaskById(int taskID) => 
            Tasks.Find(task => task.Id == taskID);

        internal void LimitColumn(string email, int limit)
        {
            if (limit != -1 && limit < Tasks.Count)
            {
                Log.Error($"New limit {limit} is less than current task count {Tasks.Count}.");
                throw new InvalidOperationException($"New limit {limit} is less than current task count {Tasks.Count}.");
            }
            Limit = limit;
            Log.Info($"Column '{Name}' limit set to {limit} by {email}.");
        }

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
                Log.Error($"Task ID {taskID} not found in column '{Name}'.");
                throw new KeyNotFoundException($"Task ID {taskID} not found in column '{Name}'.");
            }
        }
    }
}