using IntroSE.Kanban.Backend.DAL;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class TaskDTO : IDTO
    {
        internal const string TASK_TITLE_COLUMN_NAME = "title";
        internal const string TASK_DESC_COLUMN_NAME = "description";
        internal const string TASK_DUE_COLUMN_NAME = "due";
        internal const string TASK_CREATE_COLUMN_NAME = "creation_time";
        internal const string TASK_ID_COLUMN_NAME = "id";
        internal const string TASK_BOARD_ID_COLUMN_NAME = "board_id";
        internal const string TASK_COLUMN_COLUMN_NAME = "column";
        private string _title;
        private string _description;
        private DateTime _due;
        private DateTime _creationTime;
        private int _id;
        private int _boardId;
        private int _column;
        private readonly TaskController _controller;

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

        internal DateTime CreationTime => _creationTime;

        internal int Id => _id;

        internal int BoardId => _boardId;

        internal int Column
        {
            get => _column;
            set { _controller.Update(TASK_ID_COLUMN_NAME, _id, TASK_COLUMN_COLUMN_NAME, value); _column = value; }
        }

        internal TaskDTO(string title, DateTime due, string description, DateTime creationTime, int id, int boardId, int column)
        {
            _controller = new TaskController();
            _title = title;
            _description = description;
            _due = due;
            _creationTime = creationTime;
            _id = id;
            _boardId = boardId;
            _column = column;
        }

        internal void Insert()
        {
            _controller.Insert(this);
        }

        public string[] GetColumnNames() => new[] { TASK_TITLE_COLUMN_NAME, TASK_BOARD_ID_COLUMN_NAME, TASK_COLUMN_COLUMN_NAME, TASK_CREATE_COLUMN_NAME, TASK_DESC_COLUMN_NAME, TASK_DUE_COLUMN_NAME, TASK_ID_COLUMN_NAME };
        public object[] GetColumnValues() => new object[] { Title, Description, Due, CreationTime, Id, BoardId, Column };
    }
}