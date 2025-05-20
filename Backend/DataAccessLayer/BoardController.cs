using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class BoardController
    {
        private const string BOARD_TABLE_NAME = "Boards";
        private readonly string _connectionString;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            _connectionString = $"Data Source={path}; Version=3;";
        }

        internal bool Insert(BoardDTO board)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO {BOARD_TABLE_NAME} ({BoardDTO.BOARD_NAME_COLUMN_NAME}, {BoardDTO.BOARD_OWNER_COLUMN_NAME}, {BoardDTO.BOARD_ID_COLUMN_NAME}) " +
                                      "VALUES (@Name, @Owner, @Id);";
                command.Parameters.AddWithValue("@Name", board.Name);
                command.Parameters.AddWithValue("@Owner", board.Owner);
                command.Parameters.AddWithValue("@Id", board.Id);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error("Insert failed for board.", ex);
                    return false;
                }
            }
        }

        internal bool Update(int id, object newValue, string column)
        {
            // just for now an inital check of newValue's type
            if (newValue.GetType() == id.GetType())
                newValue = (int)newValue;
            else newValue = (string)newValue;
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE {BOARD_TABLE_NAME} SET {column} = @Value WHERE {BoardDTO.BOARD_ID_COLUMN_NAME} = @Id;";
                command.Parameters.AddWithValue("@Value", newValue);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error($"Update failed for board id {id}.", ex);
                    return false;
                }
            }
        }

        internal bool Delete(BoardDTO board)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM {BOARD_TABLE_NAME} WHERE {BoardDTO.BOARD_ID_COLUMN_NAME} = @Id;";
                command.Parameters.AddWithValue("@Id", board.Id);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error($"Delete failed for board id {board.Id}.", ex);
                    return false;
                }
            }
        }

        internal List<BoardDTO> SelectAll()
        {
            List<BoardDTO> boards = new List<BoardDTO>();
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM {BOARD_TABLE_NAME};";

                try
                {
                    connection.Open();
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            boards.Add(ConvertReaderToBoard(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to fetch boards.", ex);
                }
            }
            return boards;
        }

        internal BoardDTO ConvertReaderToBoard(SqliteDataReader reader)
        {
            int id = reader.GetInt32(0);
            string owner = reader.GetString(1);
            string name = reader.GetString(2);
            return new BoardDTO(name, owner, id, reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
        }
    }
}