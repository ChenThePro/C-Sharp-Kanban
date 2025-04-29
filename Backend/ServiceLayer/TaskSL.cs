using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class TaskSL
    {
        public string Title { get; set; }
        public string Due { get; set; }
        public string Description { get; set; }
        public string CreationTime { get; set; }
        public int Id { get; set; }

        public TaskSL(string title, string due, string description, string creatinTime, int id)
        {
            Title = title;
            Due = due;
            Description = description;
            CreationTime = creatinTime;
            Id = id;
        }
    }
}
