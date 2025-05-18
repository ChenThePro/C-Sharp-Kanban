using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class BoardBL
    {
        internal readonly string Owner;
        internal readonly string Name;
        internal readonly List<ColumnBL> Columns;
        internal int Id;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardBL(string name, string owner, int id)
        {
            Name = name;
            Columns = new List<ColumnBL> { new(0), new(1), new(2) };
            Owner = owner;
            Id = id;
        }

        internal TaskBL AddTask(string title, DateTime due, string description, DateTime creationTime, int id, int column)
        {
            TaskBL task = new TaskBL(title, due, description, creationTime, id);
            Columns[column].Add(task, Owner);
            return task;
        }

        internal void MoveTask(int column, int id, string email)
        {
            TaskBL task = GetTaskByIdAndColumn(id, column);
            if (task == null)
            {
                Log.Error("Task id" + id + " for " + email + " doesn't exist in " + Name + "'s " + Columns[column].GetName() + " column.");
                throw new KeyNotFoundException("Task id" + id + " for " + email + " doesn't exist in " + Name + "'s " + Columns[column].GetName() + " column.");
            }
            Columns[column + 1].Add(task, email);
            Columns[column].Delete(task, email);
            Log.Info("Task id " + task.Id + " moved from " + Columns[column].GetName() + " to " + Columns[column + 1].GetName() + " for " + email + " in board " + Name + ".");
        }

        internal void UpdateTask(string title, DateTime? due, string description, int id, string email, int column)
        {
            Columns[column].UpdateTask(title, due, description, id, email);
        }

        internal TaskBL GetTaskByIdAndColumn(int id, int column)
        {
            return Columns[column].GetTaskById(id);
        }

        internal void LimitColumn(int column, int limit, string email)
        {
            Columns[column].LimitColumn(limit, email);
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

        internal void AssignTask(int column, int id, string AssigneEmail)
        {
            Columns[column].AssignTask(id, AssigneEmail);
        }

        internal string GetUserBoards(string email)
        {
            throw new NotImplementedException();
        }

        internal string JoinBoard(string email, int boardID)
        {
            throw new NotImplementedException();
        }

        internal string LeaveBoard(string email, int boardID)
        {
            throw new NotImplementedException();
        }

        internal string GetBoardName(int boardId)
        {
            throw new NotImplementedException();
        }

        internal string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            throw new NotImplementedException();
        }
    }
}