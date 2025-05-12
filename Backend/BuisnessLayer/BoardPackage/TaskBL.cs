using log4net;
using System;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class TaskBL
    {
        internal string Title;
        internal DateTime Due;
        internal string Description;
        internal readonly DateTime CreationTime;
        internal readonly int Id;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal TaskBL(string title, DateTime due, string description, DateTime creationTime, int id)
        {
            Title = title;
            Due = due;
            Description = description;
            CreationTime = creationTime;
            Id = id;
        }

        internal void Update(string title, DateTime? due, string description, string email)
        {
            if (due.HasValue)
                if (((DateTime)due).CompareTo(CreationTime) < 0)
                {
                    Log.Error("due can't be before creation");
                    throw new InvalidOperationException("due can't be before creation");
                }
            Title = title ?? Title;
            Description = description ?? Description;
            Due = due ?? Due;
            Log.Info("task updated");
        }
    }
}