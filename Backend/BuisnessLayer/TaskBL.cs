using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BuisnessLayer
{
    internal class TaskBL
    {
        internal string title;
        internal string due;
        internal string description;
        internal string creationTime;
        internal int id;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal TaskBL(string title, string due, string description, string creationTime, int id)
        {
            this.title = title;
            this.due = due;
            this.description = description;
            this.creationTime = creationTime;
            this.id = id;
        }

        internal void Update(string title, string due, string description, int id, string email)
        {
            this.title = title;
            this.description = description;
            this.due = due;
            this.id = id;
            Log.Info("task updated");
        }
    }
}
