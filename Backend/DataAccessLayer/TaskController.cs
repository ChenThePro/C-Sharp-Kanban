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
            string assignee = reader.IsDBNull(2) ? null : reader.GetString(2);
            string description = reader.IsDBNull(6) ? null : reader.GetString(6);
            return new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), assignee, reader.GetDateTime(3),
                reader.GetDateTime(4), reader.GetString(5), description, reader.GetInt32(7));
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
                Log.Info($"GetLastId: first task for board {boardId}.");
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