using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class BoardBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal readonly string Owner;
        internal readonly string Name;
        internal int Id;
        internal readonly List<ColumnBL> Columns;

        internal BoardBL(string owner, string name, int id)
        {
            Owner = owner;
            Name = name;
            Id = id;
            Columns = new List<ColumnBL> { new(0), new(1), new(2) };
        }

        internal TaskBL AddTask(string title, string description, DateTime dueDate, DateTime created_at, int taskID, int columnOrdinal)
        {
            TaskBL task = new TaskBL(title, description, dueDate, created_at, taskID);
            Columns[columnOrdinal].Add(Owner, task);
            return task;
        }

        internal void AdvanceTask(string email, int columnOrdinal, int taskID)
        {
            TaskBL task = GetTaskByIdAndColumn(columnOrdinal, taskID);
            if (task == null)
            {
                Log.Error("Task id " + taskID + " for " + email + " doesn't exist in " + Name + "'s " + 
                    Columns[columnOrdinal].GetName() + " column.");
                throw new KeyNotFoundException("Task id" + taskID + " for " + email + " doesn't exist in " + Name + "'s " + Columns[columnOrdinal].GetName() + " column.");
            }
            Columns[columnOrdinal + 1].Add(email, task);
            Columns[columnOrdinal].Delete(email, task);
            Log.Info("Task id " + task.Id + " moved from " + Columns[columnOrdinal].GetName() + " to " + Columns[columnOrdinal + 1].GetName() + " for " + email + " in board " + Name + ".");
        }

        internal void UpdateTask(string email, int columnOrdinal, int taskID, DateTime? dueDate, string title, 
            string description)
        {
            Columns[columnOrdinal].UpdateTask(email, taskID, dueDate, title, description);
        }

        internal TaskBL GetTaskByIdAndColumn(int columnOrdinal, int taskID)
        {
            return Columns[columnOrdinal].GetTaskById(taskID);
        }

        internal void LimitColumn(string email, int columnOrdinal, int limit)
        {
            Columns[columnOrdinal].LimitColumn(email, limit);
        }

        internal List<TaskBL> GetColumn(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetTasks();
        }

        internal int GetColumnLimit(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetLimit();
        }

        internal string GetColumnName(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetName();
        }

        internal void AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee)
        {
            Columns[columnOrdinal].AssignTask(email, taskID, emailAssignee);
        }
        internal string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            throw new NotImplementedException();
        }
    }
}