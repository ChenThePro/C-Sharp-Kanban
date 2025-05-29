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
            return new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3),
                reader.GetDateTime(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7));
        }
    }

}