using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class BoardUserController : BaseController<BoardUserDTO>
    {

        internal BoardUserController() : base("BoardsUsers") { }

        protected override BoardUserDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            return new BoardUserDTO(reader.GetString(1), reader.GetInt32(0));
        }
    }
}