using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class BoardBL
    {
        internal readonly string owner;
        internal string name;
        internal List<ColumnBL> columns;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardBL(string name, string owner)
        {
            this.name = name;
            columns = new List<ColumnBL> { new(0), new(1), new(2) };
            this.owner = owner;
        }

        internal TaskBL AddTask(string title, DateTime due, string description, DateTime creationTime, int id, int column)
        {
            TaskBL task = new TaskBL(title, due, description, creationTime, id);
            columns[column].Add(task, owner);
            return task;
        }

        internal void MoveTask(int column, int id, string email)
        {
            TaskBL task = GetTaskByIdAndColumn(id, column);
            if (task == null)
                throw new KeyNotFoundException("task doesn't exist");
            columns[column + 1].Add(task, email);
            columns[column].Delete(task, email);
            Log.Info("task moved from" + task.id + "to" + column);
        }

        internal void UpdateTask(string title, DateTime? due, string description, int id, string email, int column)
        {
            columns[column].UpdateTask(title, due, description, id, email);
        }

        internal TaskBL GetTaskByIdAndColumn(int id, int column)
        {
            return columns[column].GetTaskByIdAndColumn(id);
        }

        internal void LimitColumn(int column, int limit, string email)
        {
            columns[column].LimitColumn(limit, email);
        }

        internal List<TaskBL> GetColumn(int columnOrdinal)
        {
            return columns[columnOrdinal].GetColumn();
        }

        internal int GetColumnLimit(int columnOrdinal)
        {
            return columns[columnOrdinal].GetColumnLimit();
        }

        internal string GetColumnName(int columnOrdinal)
        {
            return columns[columnOrdinal].GetColumnName();
        }
    }
}
