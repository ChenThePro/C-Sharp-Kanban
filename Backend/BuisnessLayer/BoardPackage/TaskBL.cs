using Backend.BuisnessLayer.UserPackage;
using log4net;

namespace Backend.BuisnessLayer.BoardPackage
{
    internal class TaskBL
    {
        internal string title;
        internal DateTime due;
        internal string description;
        internal readonly DateTime creationTime;
        internal int id;
        private static readonly ILog Log = LogManager.GetLogger(typeof(TaskBL));

        internal TaskBL(string title, DateTime due, string description, DateTime creationTime, int id)
        {
            this.title = title;
            this.due = due;
            this.description = description;
            this.creationTime = creationTime;
            this.id = id;
        }

        internal void Update(string? title, DateTime? due, string? description, int? id, string email)
        {
            this.title = title ??= this.title;
            this.description = description ??= this.description;
            this.due = due ??= this.due;
            this.id = id ??= this.id;
            Log.Info("task updated");
        }
    }
}
