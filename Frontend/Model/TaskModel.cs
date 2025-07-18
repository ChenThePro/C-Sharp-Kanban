﻿using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class TaskModel
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; init; }
        public string Assignee { get; set; }

        public TaskModel(TaskSL task)
        {
            Title = task.Title;
            DueDate = task.DueDate;
            CreatedAt = task.CreatedAt;
            Assignee = task.Assignee;
        }
    }
}