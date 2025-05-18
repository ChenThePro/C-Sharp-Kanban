using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class TaskDAL
    {
        public string Title;
        public string Description;
        public DateTime DueDate;
        public DateTime CreationTime;
        public int Id;
        public int BoardId;

        public TaskDAL(string title, DateTime dueDate, string description, DateTime creationTime, int id, int boardId)
        {
            this.Title = title;
            this.DueDate = dueDate;
            this.Description = description;
            this.CreationTime = creationTime;
            this.Id = id;
            this.BoardId = boardId;

        }
    }
}
