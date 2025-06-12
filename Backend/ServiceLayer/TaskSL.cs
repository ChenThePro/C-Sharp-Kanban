using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskSL
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; init; }
        public string Assignee { get; set; }

        public TaskSL(string title, DateTime due, string description, string assignee, DateTime creationTime)
        {
            Title = title;
            DueDate = due;
            Description = description;
            Assignee = assignee;
            CreatedAt = creationTime;
        }
    }
}