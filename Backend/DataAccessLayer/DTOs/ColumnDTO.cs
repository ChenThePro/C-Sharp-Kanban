using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class ColumnDTO
    {
        internal int Limit { get; set; }

        internal List<TaskDTO> Tasks { get; init; }

        private readonly TaskController _controller;

        internal ColumnDTO(int boardId, int limit, int index, bool created = false)
        {
            Limit = limit;
            _controller = new();
            if (!created)
                Tasks = _controller.SelectAll()
                    .FindAll(task => task.BoardId == boardId && task.Column == index);
            else Tasks = new();
        }

        internal void AddTask(TaskDTO task) => Tasks.Add(task);

        internal void RemoveTask(TaskDTO task) => Tasks.Remove(task);
    }
}