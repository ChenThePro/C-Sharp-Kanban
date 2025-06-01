using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardDTO : IDTO
    {
        private const string ID = "id", OWNER = "owner", NAME = "name", 
            LIMIT_0 = "limit_0", LIMIT_1 = "limit_1", LIMIT_2 = "limit_2";

        private int _id, _limit0, _limit1, _limit2;
        private string _owner, _name;

        internal int Id
        {
            get => _id;
            set { Update(ID, value); _id = value; }
        }

        internal string Owner
        {
            get => _owner;
            set { Update(OWNER, value); _owner = value; }
        }

        internal string Name
        {
            get => _name;
            set { Update(NAME, value); _name = value; }
        }

        internal List<ColumnDTO> Columns { get; init; }

        private readonly BoardController _controller;

        internal BoardDTO(string owner, string name)
        {
            _owner = owner;
            _name = name;
            _limit0 = -1;
            _limit1 = -1;
            _limit2 = -1;
            _controller = new();
            _controller.Insert(this);
            _id = _controller.GetNextId();
            new BoardUserDTO(_id, _owner).Insert();
            Columns = new()
            {
                new(_id, _limit0, 0),
                new(_id, _limit1, 1),
                new(_id, _limit2, 2)
            };
        }

        internal BoardDTO(int id, string owner, string name, int limit0, int limit1, int limit2)
        {
            _id = id;
            _owner = owner;
            _name = name;
            _limit0 = limit0;
            _limit1 = limit1;
            _limit2 = limit2;
            _controller = new();
            Columns = new()
            {
                new(_id, _limit0, 0),
                new(_id, _limit1, 1),
                new(_id, _limit2, 2)
            };
        }

        internal BoardDTO() => _controller = new();

        internal void AddTask(TaskBL task)
        {
            task.Insert();
            Columns[0].AddTask(task.TaskDTO);
        }

        internal void AdvanceTask(TaskDTO task, string email, int columnOrdinal)
        {
            task.Column++;
            Columns[columnOrdinal + 1].AddTask(task);
            Columns[columnOrdinal].RemoveTask(task);
        }

        internal void LimitColumn(int limit, int columnOrdinal)
        {
            string columnName = columnOrdinal switch
            {
                0 => LIMIT_0,
                1 => LIMIT_1,
                2 => LIMIT_2,
                _ => throw new KeyNotFoundException("Invalid column index")
            };
            _controller.Update(ID, _id, columnName, limit);
            Columns[columnOrdinal].Limit = limit;
        }

        internal void Delete()
        {
            foreach (ColumnDTO column in Columns)
                foreach (TaskDTO task in column.Tasks)
                    task.Delete();
            _controller.Delete(ID, _id);
        }

        internal List<BoardDTO> SelectAll() => _controller.SelectAll();

        public void Update(string column, object newValue) => 
            _controller.Update(ID, _id, column, newValue);

        public string[] GetColumnNames() =>
            new[] { OWNER, NAME, LIMIT_0, LIMIT_1, LIMIT_2 };

        public object[] GetColumnValues() =>
            new object[] { _owner, _name, _limit0, _limit1, _limit2 };
    }
}