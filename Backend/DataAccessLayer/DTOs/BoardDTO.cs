using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardDTO : IDTO
    {
        internal const string BOARD_ID_COLUMN_NAME = "id";
        internal const string BOARD_NAME_COLUMN_NAME = "name";
        internal const string BOARD_OWNER_COLUMN_NAME = "owner";
        internal const string BOARD_LIMIT0_COLUMN_NAME = "limit_0";
        internal const string BOARD_LIMIT1_COLUMN_NAME = "limit_1";
        internal const string BOARD_LIMIT2_COLUMN_NAME = "limit_2";
        private int _id;
        private string _name;
        private string _owner;
        private readonly List<ColumnDTO> _columns;
        private readonly BoardController _controller;

        internal int Id => _id;

        internal List<ColumnDTO> Columns => _columns;

        internal string Name
        {
            get => _name;
            set { _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_NAME_COLUMN_NAME, value); _name = value; }
        }

        internal string Owner
        {
            get => _owner;
            set { _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_OWNER_COLUMN_NAME, value); _owner = value; }
        }

        internal BoardDTO(string name, string owner, int id, int limit_0, int limit_1, int limit_2)
        {
            _controller = new BoardController();
            _name = name;
            _owner = owner;
            _id = id;
            _columns = new List<ColumnDTO> { new ColumnDTO(id, limit_0, 0), new ColumnDTO(id, limit_1, 1), new ColumnDTO(id, limit_2, 2) };
        }

        internal void AddTask(string title, string description, DateTime due, string email, int taskId)
        {
            TaskDTO task = new TaskDTO(title, due, description, DateTime.Today, taskId, _id, 0);
            task.Insert();
            _columns[0].AddTask(task);
        }

        internal void MoveTask(string title, string description, DateTime due, string email, int taskId, int columnIndex)
        {
            TaskDTO task = new TaskDTO(title, due, description, DateTime.Today, taskId, _id, columnIndex);
            task.Column = columnIndex + 1;
            _columns[columnIndex].RemoveTask(task);
            _columns[columnIndex + 1].AddTask(task);
        }

        internal void LimitColumn(int limit, int column)
        {
            switch (column)
            {
                case 0:
                    _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_LIMIT0_COLUMN_NAME, limit);
                    _columns[0].Limit = limit;
                    break;
                case 1:
                    _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_LIMIT1_COLUMN_NAME, limit);
                    _columns[1].Limit = limit;
                    break;
                case 2:
                    _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_LIMIT2_COLUMN_NAME, limit);
                    _columns[2].Limit = limit;
                    break;
            }
        }

        internal void Insert()
        {
            _controller.Insert(this);
        }

        internal void Delete()
        {
            _controller.Delete(BOARD_ID_COLUMN_NAME, Id);
        }

        internal List<BoardDTO> SelectAll()
        {
            return _controller.SelectAll();
        }

        public string[] GetColumnNames() => new[] { BOARD_NAME_COLUMN_NAME, BOARD_OWNER_COLUMN_NAME, BOARD_ID_COLUMN_NAME };
        public object[] GetColumnValues() => new object[] { Name, Owner, Id, Columns };
    }
}