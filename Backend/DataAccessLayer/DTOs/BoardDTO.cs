using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardDTO : IDTO
    {
        private const string BOARD_ID_COLUMN_NAME = "id";
        private const string BOARD_NAME_COLUMN_NAME = "name";
        private const string BOARD_OWNER_COLUMN_NAME = "owner";
        private const string BOARD_LIMIT0_COLUMN_NAME = "limit_0";
        private const string BOARD_LIMIT1_COLUMN_NAME = "limit_1";
        private const string BOARD_LIMIT2_COLUMN_NAME = "limit_2";
        private int _id;
        private string _name;
        private string _owner;
        private readonly List<ColumnDTO> _columns;
        private readonly BoardController _controller;

        internal int Id => _id;

        internal List<ColumnDTO> Columns => _columns;

        internal string Owner
        {
            get => _owner;
            set { _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_OWNER_COLUMN_NAME, value); _owner = value; }
        }

        internal string Name
        {
            get => _name;
            set { _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_NAME_COLUMN_NAME, value); _name = value; }
        }

        internal BoardDTO(int id, string owner, string name, int limit_0, int limit_1, int limit_2)
        {
            _id = id;
            _owner = owner;
            _name = name;
            _columns = new List<ColumnDTO> { new(id, limit_0, 0), new(id, limit_1, 1), new(id, limit_2, 2) };
            _controller = new BoardController();
        }

        internal BoardDTO(int id)
        {
            _id = id;
            _controller = new BoardController();
        }

        internal BoardDTO()
        {
            _controller = new BoardController();
        }

        internal void AddTask(int taskId, DateTime due, string title, string description, string email)
        {
            TaskDTO task = new TaskDTO(taskId, _id, DateTime.Today, due, title, description, 0);
            task.Insert();
            _columns[0].AddTask(task);
        }

        internal void MoveTask(string title, string description, DateTime due, string email, int taskId, int columnOrdinal)
        {
            TaskDTO task = new TaskDTO(taskId, _id, DateTime.Today, due, title, description, columnOrdinal);
            task.Column++;
            _columns[columnOrdinal].RemoveTask(task);
            _columns[columnOrdinal + 1].AddTask(task);
        }

        internal void LimitColumn(int limit, int columnOrdinal)
        {
            switch (columnOrdinal)
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
            new BoardUserDTO(_id, _owner).Insert();
        }

        internal void Delete()
        {
            _controller.Delete(BOARD_ID_COLUMN_NAME, _id);
            new BoardUserDTO(_id, _owner).Delete();
        }

        internal List<BoardDTO> SelectAll()
        {
            return _controller.SelectAll();
        }

        public string[] GetColumnNames() => new[] { BOARD_ID_COLUMN_NAME, BOARD_OWNER_COLUMN_NAME, BOARD_NAME_COLUMN_NAME, 
            BOARD_LIMIT0_COLUMN_NAME, BOARD_LIMIT1_COLUMN_NAME, BOARD_LIMIT2_COLUMN_NAME };
        public object[] GetColumnValues() => new object[] { _id, _owner, _name, _columns[0].Limit, _columns[1].Limit, 
            _columns[2].Limit };
    }
}