using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class ColumnDTO
    {
        internal int _limit;
        private int _boardId;
        private readonly List<TaskDTO> _tasks;

        internal int BoardId => _boardId;

        internal List<TaskDTO> Tasks => _tasks;

        internal ColumnDTO(int index, int boardId, int limit)
        {
            _boardId = boardId;
            _limit = limit;
            _tasks = new List<TaskDTO>();
        }

        internal void AddTask(TaskDTO task)
        {
            task.Insert();
            _tasks.Add(task);
        }
    }
}