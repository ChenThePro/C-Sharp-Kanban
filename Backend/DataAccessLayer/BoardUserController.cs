using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class BoardUserController
    {
        private const string TABLE_NAME = "BoardUsers";
        private readonly string _connectionString;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardUserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            _connectionString = $"Data Source={path}; Version=3;";
        }

        internal bool Insert(BoardUserDTO user)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO {TABLE_NAME} ({BoardUserDTO.BOARD_USER_EMAIL_COLUMN_NAME}, {BoardUserDTO.BOARD_ID_COLUMN_NAME}) VALUES (@Email, @Id);";
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Id", user.Id);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error("Insert failed for board user.", ex);
                    return false;
                }
            }
        }

        internal bool Update(int id, string newValue, string column)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE {TABLE_NAME} SET {column} = @Value WHERE {BoardUserDTO.BOARD_ID_COLUMN_NAME} = @Id;";
                command.Parameters.AddWithValue("@Value", newValue);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error($"Update failed for board user with id {id}.", ex);
                    return false;
                }
            }
        }

        internal bool Delete(BoardUserDTO user)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM {TABLE_NAME} WHERE {BoardUserDTO.BOARD_ID_COLUMN_NAME} = @Id;";
                command.Parameters.AddWithValue("@Id", user.Id);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error($"Delete failed for board user with id {user.Id}.", ex);
                    return false;
                }
            }
        }
    }
}