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

        internal int GetLastId(int boardId)
        {
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT MAX(id) FROM {_tableName} WHERE board_id = @BoardId;";
            command.Parameters.AddWithValue("@BoardId", boardId);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value && result != null && int.TryParse(result.ToString(), out int lastId))
                {
                    Log.Info($"GetLastId succeeded for board {boardId} in table {_tableName}. Last ID: {lastId}");
                    return lastId + 1;
                }
                Log.Warn($"GetLastId: No tasks found for board {boardId}.");
                return 1;
            }
            catch (Exception ex)
            {
                Log.Error($"GetLastId failed for board {boardId} in table {_tableName}.", ex);
                return -1;
            }
        }

    }

}