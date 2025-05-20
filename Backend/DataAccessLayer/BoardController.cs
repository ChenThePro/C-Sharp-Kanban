using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardController : BaseController<BoardDTO>
    {
        internal BoardController() : base("Boards") { }

        protected override BoardDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            return new BoardDTO(
                reader.GetString(2),
                reader.GetString(1),
                reader.GetInt32(0),
                reader.GetInt32(3),
                reader.GetInt32(4),
                reader.GetInt32(5)
            );
        }
    }
}
