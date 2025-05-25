using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
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