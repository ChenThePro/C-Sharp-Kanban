using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class TaskBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal string Title;
        internal string Description;
        internal DateTime DueDate;
        internal readonly DateTime CreatedAt;
        internal readonly int Id;
        internal string Assignee;
        private readonly TaskDTO _taskDTO;

        internal TaskDTO TaskDTO => _taskDTO;

        internal TaskBL(string title, string description, DateTime due, DateTime created_at, int boardId, int columnOrdinal)
        {
            Title = title;
            DueDate = due;
            Description = description;
            CreatedAt = created_at;
            Assignee = null;
            _taskDTO = new TaskDTO(boardId, null, CreatedAt, DueDate, title, description, columnOrdinal);
            Id = _taskDTO.Id;
        }

        internal TaskBL(TaskDTO taskDTO)
        {
            Title = taskDTO.Title;
            DueDate = taskDTO.DueDate;
            Description = taskDTO.Description;
            CreatedAt = taskDTO.CreationTime;
            Id = taskDTO.Id;
            Assignee = taskDTO.Assignee;
            _taskDTO = taskDTO;
        }

        internal void Update(string email, DateTime? dueDate, string title, string description)
        {
            if (email != Assignee)
            {
                Log.Error("Task can be updated only by the assignee.");
                throw new InvalidOperationException("Task can be updated only by the assignee.");
            }
            if (dueDate.HasValue)
            {
                if (((DateTime)dueDate).CompareTo(CreatedAt) < 0)
                {
                    Log.Error("Due date cannot be earlier than the creation date.");
                    throw new ArgumentOutOfRangeException("Due date cannot be earlier than the creation date.");
                }
                DueDate = (DateTime)dueDate;
                _taskDTO.DueDate = (DateTime)dueDate;
            }
            if (title != null)
            {
                Title = title;
                _taskDTO.Title = title;
            }
            if (description != null)
            {
                Description = description;
                _taskDTO.Description = description;
            }
            Log.Info("Task updated successfuly.");
        }

        internal void AssignTask(string email, string emailAssignee)
        {
            if (Assignee != null && Assignee != email)
            {
                Log.Error("Task can be assigned only by the assigne");
                throw new InvalidOperationException("Task can be assigned only by the assigne");
            }
            Assignee = emailAssignee;
            _taskDTO.Assignee = emailAssignee;
        }

        internal void Insert()
        {
            _taskDTO.Insert();
        }
    }
}