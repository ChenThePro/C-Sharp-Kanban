using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserController : SingleKeyController<UserDTO>
    {
        internal UserController() : base("Users") { }

        protected override UserDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            return new UserDTO(reader.GetString(0), reader.GetString(1), reader.GetBoolean(2), reader.GetBoolean(3));
        }
    }
}