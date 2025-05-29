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
        internal int Id;
        internal string Owner;
        internal readonly string Name;
        internal readonly List<ColumnBL> Columns;
        internal readonly List<String> Members;
        private readonly BoardDTO _boardDTO;

        internal BoardBL(string owner, string name, int id)
        {
            Id = id;
            Owner = owner;
            Name = name;
            Columns = new List<ColumnBL>{ new(0, -1, new List<TaskBL>()), new(1, -1, new List<TaskBL>()), new(2, -1, new List<TaskBL>())};            
            Members = new List<String>();
            Members.Add(owner);
            _boardDTO = new BoardDTO(Id, Owner, Name, -1, -1, -1);
            _boardDTO.Insert();
        }

        public BoardBL(BoardDTO boardDTO)
        {
            Id = boardDTO.Id;
            Owner = boardDTO.Owner;
            Name = boardDTO.Name;
            Columns = new List<ColumnBL>();
            for (int i = 0; i < 3; i++)
                Columns.Add(new ColumnBL(i, boardDTO.Columns[i].Limit, ConvertTasks(boardDTO.Columns[i].Tasks)));
            Members = new List<string>();
            Members.AddRange(new BoardUserDTO(Id).GetParticipants());
            _boardDTO = boardDTO;
        }

        private List<TaskBL> ConvertTasks(List<TaskDTO> dtos) =>
    dtos.ConvertAll(t => new TaskBL(t));


        internal TaskBL AddTask(string email, string title, string description, DateTime dueDate, DateTime created_at, int taskID, int columnOrdinal)
        {
            if (!Members.Contains(email))
            {
                Log.Error("User " + email + " is not a member of the board.");
                throw new InvalidOperationException("User " + email + " is not a member of the board.");
            }
            TaskBL task = new TaskBL(title, description, dueDate, created_at, taskID, Id, columnOrdinal);
            Columns[columnOrdinal].Add(Owner, task);
            _boardDTO.AddTask(task, email);
            return task;
        }

        internal void AdvanceTask(string email, int columnOrdinal, int taskID)
        {
            TaskBL task = GetTaskByIdAndColumn(columnOrdinal, taskID);
            if (task == null)
            {
                Log.Error("Task id " + taskID + " for " + email + " doesn't exist in " + Name + "'s " + 
                    Columns[columnOrdinal].GetName() + " column.");
                throw new KeyNotFoundException("Task id" + taskID + " for " + email + " doesn't exist in " + Name + "'s " + Columns[columnOrdinal].GetName() + " column.");
            }
            if (task.Assigne != email)
            {
                Log.Error("Task id " + taskID + " for " + email + " is not assigned to the user.");
                throw new InvalidOperationException("Task id " + taskID + " for " + email + " is not assigned to the user.");
            }
            Columns[columnOrdinal + 1].Add(email, task);
            Columns[columnOrdinal].Delete(email, task);
            // if (columnOrdinal + 1 == 2)
            //    task.AssignTask(email, null);
            _boardDTO.AdvanceTask(task, email, columnOrdinal);
            Log.Info("Task id " + task.Id + " moved from " + Columns[columnOrdinal].GetName() + " to " + Columns[columnOrdinal + 1].GetName() + " for " + email + " in board " + Name + ".");
        }

        internal void UpdateTask(string email, int columnOrdinal, int taskID, DateTime? dueDate, string title, string description)
        {
            Columns[columnOrdinal].UpdateTask(email, taskID, dueDate, title, description);
        }

        internal TaskBL GetTaskByIdAndColumn(int columnOrdinal, int taskID)
        {
            return Columns[columnOrdinal].GetTaskById(taskID);
        }

        internal void LimitColumn(string email, int columnOrdinal, int limit)
        {
            Columns[columnOrdinal].LimitColumn(email, limit);
            _boardDTO.LimitColumn(limit, columnOrdinal);
        }

        internal List<TaskBL> GetColumn(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetTasks();
        }

        internal int GetColumnLimit(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetLimit();
        }

        internal string GetColumnName(int columnOrdinal)
        {
            return Columns[columnOrdinal].GetName();
        }

        internal void AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee)
        {
            if (!Members.Contains(email))
            {
                Log.Error("User " + email + " is not a member of the board.");
                throw new InvalidOperationException("User " + email + " is not a member of the board.");
            }
            Columns[columnOrdinal].AssignTask(email, taskID, emailAssignee);
        }

        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail)
        {
            if (Owner != currentOwnerEmail)
            {
                Log.Error("Only the owner can transfer ownership.");
                throw new InvalidOperationException("Only the owner can transfer ownership.");
            }
            if (!Members.Contains(newOwnerEmail))
            {
                Log.Error("New owner must be a member of the board.");
                throw new InvalidOperationException("New owner must be a member of the board.");
            }
            Owner = newOwnerEmail;
            _boardDTO.Owner = Owner;
        }

        internal void LeaveBoard(string email)
        {
            if (Owner == email)
            {
                Log.Error("Owner cannot leave the board. Transfer ownership first.");
                throw new InvalidOperationException("Owner cannot leave the board. Transfer ownership first.");
            }
            if (!Members.Contains(email))
            {
                Log.Error("User is not a member of the board.");
                throw new InvalidOperationException("User is not a member of the board.");
            }
            Log.Info("User " + email + " left the board " + Name + ".");
            for (int i = 0; i < Columns.Count - 1; i++)
            {
                List<TaskBL> tasks = Columns[i].GetTasks();
                foreach (TaskBL task in tasks)
                    if (task.Assigne == email)
                        task.AssignTask(email, null);
            }
            Members.Remove(email);
            new BoardUserDTO(Id, email).Delete();
        }

        internal void Delete(string email)
        {
            if (Owner != email)
            {
                Log.Error("Only the owner can delete the board.");
                throw new InvalidOperationException("Only the owner can delete the board.");
            }
            Log.Info("Board " + Name + " deleted by owner " + email + ".");
            foreach (string member in Members)
                new BoardUserDTO(Id, member).Delete();
            _boardDTO.Delete();
        }

        internal void JoinBoard(string email)
        {
            if (Members.Contains(email))
            {
                Log.Error("User is already a member of the board.");
                throw new InvalidOperationException("User is already a member of the board.");
            }
            Members.Add(email);
            new BoardUserDTO(Id, email).Insert();
        }
    }
}