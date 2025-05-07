using log4net;
using System;
using System.ComponentModel.Design;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class TaskBL
    {
        internal string title;
        internal DateTime due;
        internal string description;
        internal readonly DateTime creationTime;
        internal int id;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal TaskBL(string title, DateTime due, string description, DateTime creationTime, int id)
        {
            this.title = title;
            this.due = due;
            this.description = description;
            this.creationTime = creationTime;
            this.id = id;
        }

        internal void Update(string title, DateTime? due, string description, string email)
        {
            if (due.HasValue)
                if (((DateTime) due).CompareTo(creationTime) < 0)
                    throw new InvalidOperationException("due can't be before creation");
            this.title = title ?? this.title;
            this.description = description ?? this.description;
            this.due = due ?? this.due;
            Log.Info("task updated");
        }
    }
}
