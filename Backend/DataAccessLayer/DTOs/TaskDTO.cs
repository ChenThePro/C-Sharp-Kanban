using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class TaskDTO : IDTO
    {
        internal const string TASK_ID_COLUMN_NAME = "id";
        internal const string TASK_BOARD_ID_COLUMN_NAME = "board_id";
        internal const string TASK_ASSIGNEE_COLUMN_NAME = "assignee";
        internal const string TASK_CREATE_COLUMN_NAME = "created_at";
        internal const string TASK_DUE_COLUMN_NAME = "due_date";
        internal const string TASK_TITLE_COLUMN_NAME = "title";
        internal const string TASK_DESC_COLUMN_NAME = "description";
        internal const string TASK_COLUMN_COLUMN_NAME = "column";
        private int _id;
        private int _boardId;
        private string _assignee;
        private readonly DateTime _creationTime;
        private DateTime _dueDate;
        private string _title;
        private string _description;
        private int _column;
        private readonly TaskController _controller;

        internal DateTime CreationTime => _creationTime;

        internal int Id
        {
            get => _id;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_ID_COLUMN_NAME, value); _id = value; }
        }

        internal int BoardId
        {
            get => _boardId;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_BOARD_ID_COLUMN_NAME, value); _boardId = value; }
        }

        internal string Assignee
        {
            get => _assignee;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_ASSIGNEE_COLUMN_NAME, value); _assignee = value; }
        }

        internal string Title
        {
            get => _title;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_TITLE_COLUMN_NAME, value); _title = value; }
        }

        internal string Description
        {
            get => _description;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_DESC_COLUMN_NAME, value); _description = value; }
        }

        internal DateTime DueDate
        {
            get => _dueDate;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_DUE_COLUMN_NAME, value.ToString("yyyy-MM-dd HH:mm:ss")); _dueDate = value; }
        }

        internal int Column
        {
            get => _column;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_COLUMN_COLUMN_NAME, value); _column = value; }
        }

        internal TaskDTO(int boardId, string assignee, DateTime creationTime, DateTime due, string title, string description, int column)
        {
            _boardId = boardId;
            _assignee = assignee;
            _creationTime = creationTime;
            _dueDate = due;
            _title = title;
            _description = description;
            _column = column;
            _controller = new TaskController();
            _id = _controller.GetLastId(_boardId);
        }

        internal TaskDTO(int id, int boardId, string assignee, DateTime creationTime, DateTime due, string title, string description, int column)
        {
            _id = id;
            _boardId = boardId;
            _assignee = assignee;
            _creationTime = creationTime;
            _dueDate = due;
            _title = title;
            _description = description;
            _column = column;
            _controller = new TaskController();
        }

        internal TaskDTO(int id)
        {
            _id = id;
            _controller = new TaskController();
        }

        internal TaskDTO()
        {
            _controller = new TaskController();
        }

        internal void Insert()
        {
            _controller.Insert(this);
        }

        internal void Delete()
        {
            _controller.Delete(TASK_ID_COLUMN_NAME, _id);
        }

        internal List<TaskDTO> SelectAll()
        {
            return _controller.SelectAll();
        }

        public string[] GetColumnNames() => new[] { TASK_ID_COLUMN_NAME, TASK_BOARD_ID_COLUMN_NAME, TASK_ASSIGNEE_COLUMN_NAME, 
            TASK_CREATE_COLUMN_NAME, TASK_DUE_COLUMN_NAME, TASK_TITLE_COLUMN_NAME, TASK_DESC_COLUMN_NAME, TASK_COLUMN_COLUMN_NAME };
        
        public object[] GetColumnValues() => new object[] { _id, _boardId, _assignee, 
            _creationTime.ToString("yyyy-MM-dd HH:mm:ss"), _dueDate.ToString("yyyy-MM-dd HH:mm:ss"), _title, _description, _column };
    }
}