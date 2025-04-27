using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class BoardSL
    {
        public string Name { get; set; }
        public List<TaskSL> Backlog { get; set; }
        public List<TaskSL> InProgress { get; set; }
        public List<TaskSL> Done { get; set; }

        public BoardSL(string name, List<TaskSL> backlog, List<TaskSL> inProgress, List<TaskSL> done)
        {
            Name = name;
            Backlog = backlog;
            InProgress = inProgress;
            Done = done;
        }
    }
}
