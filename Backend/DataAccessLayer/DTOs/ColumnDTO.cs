using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class ColumnDTO
    {
        internal int Limit;
        private readonly int _boardId;
        private readonly List<TaskDTO> _tasks;
        private readonly TaskController _controller;


        internal List<TaskDTO> Tasks => _tasks;

        internal ColumnDTO(int id, int limit, int index)
        {
            _boardId = id;
            Limit = limit;
            _controller = new TaskController();
            _tasks = _controller.SelectAll().FindAll(task =>  task.BoardId == _boardId && task.Column == index);
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