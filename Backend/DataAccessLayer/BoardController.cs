using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardController : SingleKeyController<BoardDTO>
    {
        internal BoardController() : base("Boards") { }

        internal int GetNextId()
        {
            int nextID = -1;
            try
            {
                using SqliteConnection connection = new(_connectionString);
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT seq FROM sqlite_sequence WHERE name = '{_tableName}';";
                connection.Open();
                object result = command.ExecuteScalar();
                nextID = Convert.ToInt32(result);
                Log.Info($"GetLastId succeeded for table {_tableName}. Last ID: {nextID}");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return nextID;
        }

        protected override BoardDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            return new BoardDTO(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetInt32(3), 
                reader.GetInt32(4), reader.GetInt32(5));
        }
    }
}
