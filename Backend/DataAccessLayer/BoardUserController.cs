using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardUserController : CompositeKeyController<BoardUserDTO>
    {

        internal BoardUserController() : base("BoardsUsers") { }

        internal List<string> GetParticipants(string keyColumn, int key)
        {
            List<string> results = new();
            try
            {
                using SqliteConnection connection = new(_connectionString);
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {_tableName} WHERE {keyColumn} = @key;";
                command.Parameters.AddWithValue("@key", key);
                connection.Open();
                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    results.Add(reader.GetString(1));
                Log.Info($"Select succeeded on {_tableName}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return results;
        }

        internal List<int> GetBoards(string keyColumn, string key)
        {
            List<int> results = new();
            try
            {
                using SqliteConnection connection = new(_connectionString);
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {_tableName} WHERE {keyColumn} = @key;";
                command.Parameters.AddWithValue("@key", key);
                connection.Open();
                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    results.Add(reader.GetInt32(0));
                Log.Info($"Select succeeded on {_tableName}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return results;
        }

        protected override BoardUserDTO ConvertReaderToDTO(SqliteDataReader reader)
        {
            return new BoardUserDTO(reader.GetInt32(0), reader.GetString(1));
        }
    }
}