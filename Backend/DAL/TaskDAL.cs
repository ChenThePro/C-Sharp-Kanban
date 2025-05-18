using System;

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
            Title = title;
            DueDate = dueDate;
            Description = description;
            CreationTime = creationTime;
            Id = id;
            BoardId = boardId;
        }
    }
}
