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
                Log.Error("task " + id + " for " + email + " doesn't exist in " + Name + "'s " + Columns[column].GetColumnName());
                throw new KeyNotFoundException("task doesn't exist");
            }
            Columns[column + 1].Add(task, email);
            Columns[column].Delete(task, email);
            Log.Info("task " + task.Id + " moved from " + Columns[column].GetColumnName() + " to " + Columns[column + 1].GetColumnName() + " for " + email);
        }

        internal void UpdateTask(string title, DateTime? due, string description, int id, string email, int column)
        {
            Columns[column].UpdateTask(title, due, description, id, email);
        }

        internal TaskBL GetTaskByIdAndColumn(int id, int column)
        {
            return Columns[column].GetTaskByIdAndColumn(id);
        }

        internal void LimitColumn(int column, int limit, string email)
        {
            Columns[column].LimitColumn(limit, email);
        }

        internal List<TaskBL> GetColumn(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetColumn();
        }

        internal int GetColumnLimit(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetColumnLimit();
        }

        internal string GetColumnName(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetColumnName();
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
        internal string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
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