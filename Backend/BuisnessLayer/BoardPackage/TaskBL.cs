using log4net;
using System;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class TaskBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal string Title;
        internal string Description;
        internal DateTime Due;
        internal readonly DateTime CreatedAt;
        internal readonly int Id;
        internal string Assigne;

        internal TaskBL(string title, string description, DateTime due, DateTime created_at, int taskID)
        {
            Title = title;
            Due = due;
            Description = description;
            CreatedAt = created_at;
            Id = taskID;
            Assigne = null;
        }

        internal void Update(string email, DateTime? due, string title, string description)
        {
            if (due.HasValue)
                if (((DateTime)due).CompareTo(CreatedAt) < 0)
                {
                    Log.Error("Due date cannot be earlier than the creation date.");
                    throw new ArgumentOutOfRangeException("Due date cannot be earlier than the creation date.");
                }
            if (email != Assigne)
            {
                Log.Error("Task can be updated only by the assignee.");
                throw new InvalidOperationException("Task can be updated only by the assignee.");
            }
            Title = title ?? Title;
            Description = description ?? Description;
            Due = due ?? Due;
            Log.Info("Task updated successfuly.");
        }

        internal void AssignTask(string email, string emailAssignee)
        {
            if (Assigne != null && Assigne != email)
            {
                Log.Error("Task can be assigned only by the assigne");
                throw new InvalidOperationException("Task can be assigned only by the assigne");
            }
            Assigne = emailAssignee;
        }
    }
}