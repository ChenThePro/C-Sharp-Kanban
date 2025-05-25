using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class TaskDTO : IDTO
    {
        internal const string TASK_ID_COLUMN_NAME = "id";
        internal const string TASK_BOARD_ID_COLUMN_NAME = "board_id";
        internal const string TASK_CREATE_COLUMN_NAME = "created_at";
        internal const string TASK_DUE_COLUMN_NAME = "due";
        internal const string TASK_TITLE_COLUMN_NAME = "title";
        internal const string TASK_DESC_COLUMN_NAME = "description";
        internal const string TASK_COLUMN_COLUMN_NAME = "column";
        private int _id;
        private int _boardId;
        private readonly DateTime _creationTime;
        private DateTime _due;
        private string _title;
        private string _description;
        private int _column;
        private readonly TaskController _controller;

        internal DateTime CreationTime => _creationTime;

        internal int Id => _id;

        internal int BoardId => _boardId;

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

        internal DateTime Due
        {
            get => _due;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_DUE_COLUMN_NAME, value.ToUniversalTime().ToString("o")); _due = value; }
        }

        internal int Column
        {
            get => _column;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_COLUMN_COLUMN_NAME, value); _column = value; }
        }

        internal TaskDTO(int id, int boardId, DateTime creationTime, DateTime due, string title, string description, int column)
        {
            _id = id;
            _boardId = boardId;
            _creationTime = creationTime;
            _due = due;
            _title = title;
            _description = description;
            _column = column;
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

        public string[] GetColumnNames() => new[] { TASK_ID_COLUMN_NAME, TASK_BOARD_ID_COLUMN_NAME, TASK_DESC_COLUMN_NAME, 
            TASK_CREATE_COLUMN_NAME, TASK_TITLE_COLUMN_NAME, TASK_DUE_COLUMN_NAME, TASK_COLUMN_COLUMN_NAME };
        public object[] GetColumnValues() => new object[] { _id, _boardId, _description, 
            _creationTime.ToUniversalTime().ToString("o"), _title, _due.ToUniversalTime().ToString("o"), _column };
    }
}