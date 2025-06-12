using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardSL
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public List<string> Members { get; set; }
        public List<ColumnSL> Columns { get; set; }

        public BoardSL(string owner, string name, List<string> members, List<ColumnSL> columns)
        {
            Owner = owner;
            Name = name;
            Members = members;
            Columns = columns;
        }
    }
}