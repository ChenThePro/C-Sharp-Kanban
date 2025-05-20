using IntroSE.Kanban.Backend.DAL;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardDTO
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

        internal int Id { get => _id; }

        internal List<ColumnDTO> Columns => _columns;

        internal string Name
        {
            get => _name;
            set { _controller.Update(_id, value, BOARD_NAME_COLUMN_NAME); _name = value; }
        }

        internal string Owner
        {
            get => _owner;
            set { _controller.Update(_id, value, BOARD_OWNER_COLUMN_NAME); _owner = value; }
        }

        internal BoardDTO(string name, string owner, int id, int limit_0, int limit_1, int limit_2)
        {
            _controller = new BoardController();
            _name = name;
            _owner = owner;
            _id = id;
            _columns = new List<ColumnDTO> { new ColumnDTO(0, id, limit_0), new ColumnDTO(1, id, limit_1), new ColumnDTO(2, id, limit_2) };
        }

        internal void AddTask(string title, string description, DateTime due, string email, int taskId, int columnIndex)
        {
            TaskDTO task = new TaskDTO(title, due, description, DateTime.Today, taskId, _id, columnIndex);
            _columns[columnIndex].AddTask(task);
        }

        internal void LimitColumn(int limit, int column)
        {
            switch (column)
            {
                case 0:
                    _controller.Update(_id, limit, BOARD_LIMIT0_COLUMN_NAME);
                    _columns[0]._limit = limit;
                    break;
                case 1:
                    _controller.Update(_id, limit, BOARD_LIMIT1_COLUMN_NAME);
                    _columns[1]._limit = limit;
                    break;
                case 2:
                    _controller.Update(_id, limit, BOARD_LIMIT2_COLUMN_NAME);
                    _columns[2]._limit = limit;
                    break;
            }
        }
    }
}