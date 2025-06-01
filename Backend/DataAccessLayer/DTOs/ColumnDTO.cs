using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class ColumnDTO
    {
        internal int Limit { get; set; }

        internal List<TaskDTO> Tasks { get; init; }

        private readonly TaskController _controller;

        internal ColumnDTO(int boardId, int limit, int index)
        {
            Limit = limit;
            _controller = new();
            Tasks = _controller.SelectAll()
                .FindAll(task => task.BoardId == boardId && task.Column == index);
        }

        internal void AddTask(TaskDTO task) => Tasks.Add(task);

        internal void RemoveTask(TaskDTO task) => Tasks.Remove(task);
    }
}