using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardUserController : BaseController<BoardUserDTO>
    {

        internal BoardUserController() : base("BoardsUsers") { }

        internal bool Delete(string keyColumn1, object key1, string keyColumn2, object key2)
        {
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {_tableName} WHERE {keyColumn1} = @Key1 AND {keyColumn2} = @Key2;";
            command.Parameters.AddWithValue("@Key1", key1);
            command.Parameters.AddWithValue("@Key2", key2);
            try
            {
                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    Log.Info($"Delete succeeded on {_tableName} for keys {key1}, {key2}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Delete failed on {_tableName} for keys {key1}, {key2}.", ex);
                return false;
            }
        }

        protected override BoardUserDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            return new BoardUserDTO(reader.GetInt32(0), reader.GetString(1));
        }
    }
}