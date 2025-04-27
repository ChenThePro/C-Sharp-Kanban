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
        internal TaskSL AddTask(string boardName, string title, string description, string due, int id, string email)
        {
            throw new NotImplementedException();
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
