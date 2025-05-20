using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class UserController
    {
        private const string USER_TABLE_NAME = "Users";
        private readonly string _connectionString;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal List<UserDTO> SelectAll => GetAllUsers();

        internal UserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            _connectionString = $"Data Source={path}; Version=3;";
        }

        /// <summary>
        /// Returns all users from the database.
        /// </summary>
        private List<UserDTO> GetAllUsers()
        {
            return SelectUsers().Cast<UserDTO>().ToList();
        }

        /// <summary>
        /// Select all users.
        /// </summary>
        internal List<UserDTO> SelectUsers()
        {
            List<UserDTO> users = new List<UserDTO>();
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM {USER_TABLE_NAME};";
                try
                {
                    connection.Open();
                    using (SqliteDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                            users.Add(ConvertReaderToUser(reader));
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to select users from database.", ex);
                }
            }
            return users;
        }

        /// <summary>
        /// Inserts a user into the database.
        /// </summary>
        internal bool Insert(UserDTO user)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    $"INSERT INTO {USER_TABLE_NAME} ({UserDTO.USER_EMAIL_COLUMN_NAME}, {UserDTO.USER_PASSWORD_COLUMN_NAME}) " +
                    "VALUES (@Email, @Password);";
                SqliteParameter emailParam = new SqliteParameter("@Email", user.Email);
                SqliteParameter passwordParam = new SqliteParameter("@Password", user.Password);
                command.Parameters.Add(emailParam);
                command.Parameters.Add(passwordParam);
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error($"Insert failed for user {user.Email}.", ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        internal bool Delete(UserDTO user)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM {USER_TABLE_NAME} WHERE {UserDTO.USER_EMAIL_COLUMN_NAME} = @Email;";
                SqliteParameter emailParam = new SqliteParameter("@Email", user.Email);
                command.Parameters.Add(emailParam);
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error($"Delete failed for user {user.Email}.", ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Updates a single column for a user specified by email.
        /// </summary>
        internal bool Update(string email, string newValue, string column)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE {USER_TABLE_NAME} SET {column} = @Value WHERE {UserDTO.USER_EMAIL_COLUMN_NAME} = @Email;";
                SqliteParameter valueParam = new SqliteParameter("@Value", newValue);
                SqliteParameter emailParam = new SqliteParameter("@Email", email);
                command.Parameters.Add(valueParam);
                command.Parameters.Add(emailParam);
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error($"Update failed for user {email}.", ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Converts a SqliteDataReader to a UserDTO object.
        /// </summary>
        internal UserDTO ConvertReaderToUser(SqliteDataReader reader)
        {
            return new UserDTO(reader.GetString(0), reader.GetString(1));
        }
    }
}