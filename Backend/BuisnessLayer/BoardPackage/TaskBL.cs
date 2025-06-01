using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class TaskBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string _title, _description, _assignee;
        private DateTime _dueDate;
        private int _id;

        internal string Title { 
            get => _title; 
            private set { _title = value; _taskDTO.Title = value; } 
        }

        internal string Description { 
            get => _description; 
            private set { _description = value; _taskDTO.Description = value; } 
        }

        internal DateTime DueDate { 
            get => _dueDate; 
            private set { _dueDate = value; _taskDTO.DueDate = value; } 
        }

        internal DateTime CreatedAt { get; }

        internal int Id { 
            get => _id; 
            private set { _id = value; _taskDTO.Id = value; } 
        }

        internal string Assignee { 
            get => _assignee; 
            private set { _assignee = value; _taskDTO.Assignee = value; } 
        }

        internal TaskDTO TaskDTO => _taskDTO;

        private readonly TaskDTO _taskDTO;

        internal TaskBL(string title, string description, DateTime dueDate, DateTime createdAt, int boardId, int columnOrdinal)
        {
            _title = title;
            _description = description;
            _dueDate = dueDate;
            CreatedAt = createdAt;
            _assignee = null;
            _taskDTO = new(boardId, null, CreatedAt, _dueDate, _title, _description, columnOrdinal);
            _id = _taskDTO.Id;
        }

        internal TaskBL(TaskDTO taskDTO)
        {
            _title = taskDTO.Title;
            _description = taskDTO.Description;
            _dueDate = taskDTO.DueDate;
            CreatedAt = taskDTO.CreatedAt;
            _id = taskDTO.Id;
            _assignee = taskDTO.Assignee;
            _taskDTO = taskDTO;
        }

        internal void Update(string email, DateTime? dueDate, string title, string description)
        {
            if (email != _assignee)
            {
                Log.Error("Only the assignee can update the task.");
                throw new InvalidOperationException("Only the assignee can update the task.");
            }
            if (dueDate.HasValue)
            {
                if (((DateTime)dueDate).CompareTo(CreatedAt) < 0)
                {
                    Log.Error("Due date cannot be earlier than the creation date.");
                    throw new ArgumentOutOfRangeException("Due date cannot be earlier than the creation date.");
                }
                DueDate = (DateTime)dueDate;
            }
            if (title != null)
                Title = title;
            if (description != null)
                Description = description;
            Log.Info($"Task {Id} updated by {email}.");
        }

        internal void AssignTask(string email, string emailAssignee)
        {
            if (_assignee != null && _assignee != email)
            {
                Log.Error("Only the current assignee can reassign the task.");
                throw new InvalidOperationException("Only the current assignee can reassign the task.");
            }
            Assignee = emailAssignee;
            Log.Info($"Task {Id} assigned to '{emailAssignee}' by '{email}'.");
        }

        internal void Insert() => _taskDTO.Insert();
    }
}