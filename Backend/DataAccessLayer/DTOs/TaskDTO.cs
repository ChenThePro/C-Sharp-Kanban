using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class TaskDTO : IDTO
    {
        private const string ID = "id", BOARD_ID = "board_id", ASSIGNEE = "assignee",
            CREATED_AT = "created_at", DUE_DATE = "due_date", TITLE = "title", 
            DESC = "description", COLUMN = "column";

        private int _id, _boardId, _column;
        private string _assignee, _title, _description;
        private readonly DateTime _createdAt;
        private DateTime _dueDate;

        internal DateTime CreatedAt => _createdAt;

        internal int Id
        {
            get => _id;
            set { Update(ID, value); _id = value; }
        }

        internal int BoardId
        {
            get => _boardId;
            set { Update(BOARD_ID, value); _boardId = value; }
        }

        internal string Assignee
        {
            get => _assignee;
            set { Update(ASSIGNEE, value); _assignee = value; }
        }

        internal string Title
        {
            get => _title;
            set { Update(TITLE, value); _title = value; }
        }

        internal string Description
        {
            get => _description;
            set { Update(DESC, value); _description = value; }
        }

        internal DateTime DueDate
        {
            get => _dueDate;
            set { Update(DUE_DATE, value.ToString("yyyy-MM-dd HH:mm:ss")); _dueDate = value; }
        }

        internal int Column
        {
            get => _column;
            set { Update(COLUMN, value); _column = value; }
        }

        private readonly TaskController _controller;

        internal TaskDTO(int boardId, string assignee, DateTime createdAt, DateTime due, string title, string description, int column)
        {
            _boardId = boardId;
            _assignee = assignee;
            _createdAt = createdAt;
            _dueDate = due;
            _title = title;
            _description = description;
            _column = column;
            _controller = new();
            _id = _controller.GetNextId(boardId);
        }

        internal TaskDTO(int id, int boardId, string assignee, DateTime createdAt, DateTime due, string title, string description, int column)
        {
            _id = id;
            _boardId = boardId;
            _assignee = assignee;
            _createdAt = createdAt;
            _dueDate = due;
            _title = title;
            _description = description;
            _column = column;
            _controller = new();
        }

        internal void Insert() => _controller.Insert(this);

        internal void Delete() => _controller.Delete(ID, _id, BOARD_ID, _boardId);

        public void Update(string column, object newValue) =>
            _controller.Update(ID, _id, BOARD_ID, _boardId, column, newValue);

        public string[] GetColumnNames() => 
            new[] { ID, BOARD_ID, ASSIGNEE, CREATED_AT, DUE_DATE, TITLE, DESC, COLUMN };

        public object[] GetColumnValues() =>
            new object[] { _id, _boardId, _assignee, _createdAt.ToString("yyyy-MM-dd HH:mm:ss"), _dueDate.ToString("yyyy-MM-dd HH:mm:ss"), _title, _description, _column };
    }
}