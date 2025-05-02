using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;

namespace Backend.ServiceLayer
{
    public class TaskSL
    {
        public string Title { get; set; }
        public string Due { get; set; }
        public string Description { get; set; }
        public string CreationTime { get; init; }
        public int Id { get; set; }
        internal TaskSL(TaskBL task)
        {
            Title = task.title;
            Due = task.due;
            Description = task.description;
            CreationTime = task.creationTime;
            Id = task.id;
        }
    }
}
