using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskController : BaseController<TaskDTO>
    {

        internal TaskController() : base("Tasks") { }

        protected override TaskDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            return new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), DateTime.Parse(reader.GetString(2)), 
                DateTime.Parse(reader.GetString(3)), reader.GetString(4), reader.GetString(5), reader.GetInt32(6));
        }
    }
}