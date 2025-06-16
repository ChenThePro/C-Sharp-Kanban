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

        public TaskSL(string title, string description, DateTime dueDate, DateTime createdAt, string assignee)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            CreatedAt = createdAt;
            Assignee = assignee;
        }
    }
}