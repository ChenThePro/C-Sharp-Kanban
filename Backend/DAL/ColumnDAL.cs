using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class ColumnDAL
    {
        public string Name;
        public int Limit;
        public int BoardId;
        public List<TaskDAL> Tasks;
        public ColumnDAL(int num, int boardId)
        {
            switch (num)
            {
                case 0:
                    Name = "backlog";
                    break;
                case 1:
                    Name = "in progress";
                    break;
                case 2:
                    Name = "done";
                    break;
            }
            this.Limit = -1 ;
            Tasks = new List<TaskDAL>();
            this.BoardId = boardId;

        }
        public void AddTask(TaskDAL task, string email)
        {
            throw new NotImplementedException();
        }

    }
}
