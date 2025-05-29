using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskSL
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; init; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        public TaskSL(string title, DateTime due, string description, DateTime creationTime, int id)
        {
            Title = title;
            DueDate = due;
            Description = description;
            CreationTime = creationTime;
            Id = id;
        }
    }
}