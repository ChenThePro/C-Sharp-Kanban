using IntroSE.Kanban.Backend.DAL;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class ColumnDTO
    {
        internal int Limit;
        private readonly int _boardId;
        private readonly List<TaskDTO> _tasks;
        private readonly TaskController _controller;

        internal int BoardId => _boardId;

        internal List<TaskDTO> Tasks => _tasks;

        internal ColumnDTO(int boardId, int limit, int index)
        {
            Limit = limit;
            _boardId = boardId;
            _controller = new TaskController();
            _tasks = _controller.SelectAll().FindAll(task =>  task.BoardId == BoardId && task.Column == index);
        }

        internal void AddTask(TaskDTO task)
        {
            _tasks.Add(task);
        }

        internal void RemoveTask(TaskDTO task)
        {
            _tasks.Remove(task);
        }
    }
}