using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskController : CompositeKeyController<TaskDTO>
    {

        internal TaskController() : base("Tasks") { }

        internal int GetNextId(int boardId)
        {
            int nextID = 1;
            try
            {
                using SqliteConnection connection = new(_connectionString);
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT MAX(id) FROM {_tableName} WHERE board_id = @BoardId;";
                command.Parameters.AddWithValue("@BoardId", boardId);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    nextID = Convert.ToInt32(result) + 1;
                    Log.Info($"GetNextId succeeded for board {boardId} in table {_tableName}. Next ID: {nextID}");
                }
                else Log.Info($"GetNextId: first task for board {boardId}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return nextID;
        }

        protected override TaskDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            string assignee = reader.IsDBNull(2) ? null : reader.GetString(2);
            string description = reader.IsDBNull(6) ? null : reader.GetString(6);
            return new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), assignee, reader.GetDateTime(3),
                reader.GetDateTime(4), reader.GetString(5), description, reader.GetInt32(7));
        }
    }
}