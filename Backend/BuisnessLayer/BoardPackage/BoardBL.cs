using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage
{
    internal class BoardBL
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private int _id;
        private string _owner, _name;

        internal int Id { 
            get => _id; 
            private set { _id = value; _boardDTO.Id = value; } 
        }

        internal string Owner
        {
            get => _owner;
            private set { _owner = value; _boardDTO.Owner = value; }
        }

        internal string Name { 
            get => _name; 
            private set { _name = value; _boardDTO.Name = value; } 
        }

        internal List<string> Members { get; init; }
        internal List<ColumnBL> Columns { get; init; }

        private readonly BoardDTO _boardDTO;

        internal BoardBL(string owner, string name)
        {
            _owner = owner;
            _name = name;
            Columns = InitializeDefaultColumns();
            Members = new() { owner };
            _boardDTO = new(owner, name);
            _id = _boardDTO.Id;
            Log.Info($"Board '{_name}' created for owner '{_owner}'.");
        }

        internal BoardBL(BoardDTO boardDTO)
        {
            _id = boardDTO.Id;
            _owner = boardDTO.Owner;
            _name = boardDTO.Name;
            Columns = InitializeColumnsFromDTO(boardDTO);
            Members = new(new BoardUserDTO(_id).GetParticipants());
            _boardDTO = boardDTO;
            Log.Info($"Board '{_name}' loaded from persistence.");
        }

        internal TaskBL AddTask(string email, string title, string description, DateTime dueDate, DateTime createdAt)
        {
            TaskBL task = new(title, description, dueDate, createdAt, _id, 0);
            Columns[0].AddTask(_owner, task);
            _boardDTO.AddTask(task);
            Log.Info($"Task '{task.Title}' added to column {0} by '{email}' in board {_name}'.");
            return task;
        }

        internal void AdvanceTask(string email, int columnOrdinal, int taskID)
        {
            TaskBL task = Columns[columnOrdinal].GetTaskById(taskID);
            Columns[columnOrdinal].TaskExists(task, taskID);
            if (task.Assignee != email)
            {
                Log.Error($"Task ID {taskID} is not assigned to '{email}'.");
                throw new InvalidOperationException($"Task ID {taskID} is not assigned to '{email}'.");
            }
            Columns[columnOrdinal + 1].AddTask(email, task);
            Columns[columnOrdinal].DeleteTask(email, task);
            _boardDTO.AdvanceTask(task.TaskDTO, email, columnOrdinal);
            Log.Info($"Task ID {taskID} advanced by '{email}' from column {columnOrdinal} to {columnOrdinal + 1} in board '{_name}'.");
        }

        internal void UpdateTask(string email, int columnOrdinal, int taskID, DateTime? dueDate, string title, string description)
        {
            Columns[columnOrdinal].UpdateTask(email, taskID, dueDate, title, description);
            Log.Info($"Task ID {taskID} updated by '{email}' in column {columnOrdinal} of board '{_name}'.");
        }

        internal void LimitColumn(string email, int columnOrdinal, int limit)
        {
            Columns[columnOrdinal].LimitColumn(email, limit);
            _boardDTO.LimitColumn(limit, columnOrdinal);
            Log.Info($"Column {columnOrdinal} limit set to {limit} by '{email}' in board '{_name}'.");
        }

        internal List<TaskBL> GetColumnTasks(int columnOrdinal) =>
            Columns[columnOrdinal].Tasks;

        internal int GetColumnLimit(int columnOrdinal) =>
            Columns[columnOrdinal].Limit;

        internal string GetColumnName(int columnOrdinal) =>
            Columns[columnOrdinal].Name;

        internal void AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee)
        {
            IsMember(emailAssignee);
            Columns[columnOrdinal].AssignTask(email, taskID, emailAssignee);
            Log.Info($"Task ID {taskID} assigned to '{emailAssignee}' by '{email}' in column {columnOrdinal} of board '{_name}'.");
        }

        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail)
        {
            IsOwner(currentOwnerEmail);
            IsMember(newOwnerEmail);
            Owner = newOwnerEmail;
            Log.Info($"Ownership of board '{_name}' transferred from '{currentOwnerEmail}' to '{newOwnerEmail}'.");
        }

        internal void Leave(string email)
        {
            if (_owner == email)
            {
                Log.Error(email + " cannot leave the board. Transfer ownership first.");
                throw new InvalidOperationException(email + " cannot leave the board. Transfer ownership first.");
            }
            IsMember(email);
            for (int i = 0; i < Columns.Count - 1; i++)
                foreach (TaskBL task in Columns[i].Tasks)
                    if (task.Assignee == email)
                        task.Assign(email, null);
            Members.Remove(email);
            new BoardUserDTO(_id, email).Delete();
            Log.Info($"User '{email}' left the board '{_name}'.");
        }

        internal void Delete(string email)
        {
            IsOwner(email);
            foreach (string member in Members)
                new BoardUserDTO(_id, member).Delete();
            _boardDTO.Delete();
            Log.Info($"Board '{_name}' deleted by '{email}'.");
        }

        internal void Join(string email)
        {
            if (Members.Contains(email))
            {
                Log.Error($"User '{email}' is already a member of board '{_name}'.");
                throw new InvalidOperationException($"User '{email}' is already a member of board '{_name}'.");
            }
            Members.Add(email);
            new BoardUserDTO(_id, email).Insert();
            Log.Info($"User '{email}' joined board '{_name}'.");
        }

        private List<ColumnBL> InitializeDefaultColumns()
        {
            List<ColumnBL> columns = new();
            for (int i = 0; i < 3; i++)
                columns.Add(new(i, -1, new()));
            return columns;
        }

        private List<ColumnBL> InitializeColumnsFromDTO(BoardDTO boardDTO)
        {
            List<ColumnBL> columns = new();
            for (int i = 0; i < 3; i++)
            {
                ColumnDTO dtoColumn = boardDTO.Columns[i];
                columns.Add(new(i, dtoColumn.Limit, ConvertTasks(dtoColumn.Tasks)));
            }
            return columns;
        }

        private List<TaskBL> ConvertTasks(List<TaskDTO> dtos) => 
            dtos.ConvertAll(dto => new TaskBL(dto));

        private void IsMember(string email)
        {
            if (!Members.Contains(email))
            {
                Log.Error($"User '{email}' is not a member of the board '{_name}'.");
                throw new InvalidOperationException($"User '{email}' is not a member of the board '{_name}'.");
            }
        }

        private void IsOwner(string email)
        {
            if (_owner != email)
            {
                Log.Error($"User '{email}' is not the owner of the board '{_name}'.");
                throw new InvalidOperationException($"User '{email}' is not the owner of the board '{_name}'.");
            }
        }
    }
}