using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ColumnSL
    {
        public string Name { get; set; }
        public int Limit { get; set; }
        public List<TaskSL> Tasks { get; set; }

        public ColumnSL(string name, int limit, List<TaskSL> tasks)
        {
            Name = name;
            Limit = limit;
            Tasks = tasks;
        }
    }
}
