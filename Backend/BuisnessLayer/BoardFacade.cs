using Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BuisnessLayer
{
    internal class BoardFacade
    {
       
        private Dictionary<string, BoardSL> boards;
      
        public BoardFacade()
        {
            boards = new Dictionary<string, BoardSL>();
        }
        /// <summary>
        /// helper function to check if board exist
        /// </summary>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public bool BoardExists(string boardName)
        {
            return boards.ContainsKey(boardName);
        }

        /// <summary>
        /// helper function to get board by name;
        /// </summary>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public BoardSL GetBoardByName(string boardName)
        {
            if (!BoardExists(boardName))
            {
                throw new KeyNotFoundException("Board not found.");
            }

            return boards[boardName];
        }



        /// <summary>
        /// helper function to check if task name already exists in this board;
        /// </summary>
        /// <param name="boardName"></param>
        /// <param name="taskTitle"></param>
        /// <param name="boards"></param>
        /// <returns></returns>
        bool TaskExists(string boardName, string taskTitle, Dictionary<string, BoardSL> boards)
        {
            if (!boards.ContainsKey(boardName)) 
                return false; 

            BoardSL board = boards[boardName]; 

            foreach (TaskSL task in board.Backlog) 
            {
                if (task.Title == taskTitle) 
                    return true; 
            }

            foreach (TaskSL task in board.InProgress)
            {
                if (task.Title == taskTitle) 
                    return true;
            }

            foreach (TaskSL task in board.Done)
            {
                if (task.Title == taskTitle) 
                    return true;
            }

            return false; 
        }

        /// <summary>
        /// The AddTask function adds a task to a board after validating input, 
        /// checking if the board exists and ensuring the task name isn't already taken. 
        /// If valid, it creates a new TaskSL object and adds it to the Backlog
        /// </summary>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="due"></param>
        /// <param name="id"></param>
        /// <param name="creatinTime"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        internal TaskSL AddTask(string boardName, string title, string description, string due, int id, string creatinTime, string email)
        {
            if (boardName == null || title == null)
            {
                throw new ArgumentNullException("boardname and title can't be null or empty");
            }
            if (!boards.ContainsKey(boardName))
            {
                throw new KeyNotFoundException("Board not found.");
            }
            if (TaskExists(boardName, title, boards))
            {
                throw new Exception("task name is already taken in this board");
            }
            else
            {
                TaskSL newTask = new TaskSL(title, due, description,creatinTime, id);
                boards[boardName].Backlog.Add(newTask);
                return newTask;
            }
           
        }

        internal BoardSL CreateBoard(string boardName, string email)
        {
            throw new NotImplementedException();
        }

        internal void DeleteBoard(string boardName, string email)
        {
            throw new NotImplementedException();
        }

        internal void LimitColumn(string boardName, int column, int limit, string email)
        {
            throw new NotImplementedException();
        }

        internal void MoveTask(string boardName, string column, int id, string email)
        {
            throw new NotImplementedException();
        }

        internal void UpdateTask(string boardName, string title, string description, string due, int id, string email, string column)
        {
         
            
            throw new NotImplementedException();
        }
    }
}
