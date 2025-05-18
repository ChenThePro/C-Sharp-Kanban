using System;
using System.Collections.Generic;

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
            Limit = -1 ;
            Tasks = new List<TaskDAL>();
            BoardId = boardId;

        }
        public void AddTask(TaskDAL task, string email)
        {
            throw new NotImplementedException();
        }

    }
}
