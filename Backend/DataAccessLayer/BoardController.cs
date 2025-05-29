using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardController : BaseController<BoardDTO>
    {
        internal BoardController() : base("Boards") { }

        protected override BoardDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            return new BoardDTO(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetInt32(3), 
                reader.GetInt32(4), reader.GetInt32(5));
        }

        internal int GetLastId()
        {
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT seq FROM sqlite_sequence WHERE name = '{_tableName}';";
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int lastId))
                {
                    Log.Info($"GetLastId succeeded for table {_tableName}. Last ID: {lastId}");
                    return lastId;
                }
                Log.Warn($"GetLastId: No entry found for table {_tableName} in sqlite_sequence.");
                return -1;
            }
            catch (Exception ex)
            {
                Log.Error($"GetLastId failed for table {_tableName}.", ex);
                return -1;
            }
        }
    }
}
