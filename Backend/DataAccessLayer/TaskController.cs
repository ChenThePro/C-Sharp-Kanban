using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class TaskController
    {
        private const string TASK_TABLE_NAME = "Tasks";
        private readonly string _connectionString;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal TaskController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            _connectionString = $"Data Source={path}; Version=3;";
        }

        internal bool Insert(TaskDTO task)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO {TASK_TABLE_NAME} " +
                    $"({TaskDTO.TASK_TITLE_COLUMN_NAME}, {TaskDTO.TASK_DESC_COLUMN_NAME}, {TaskDTO.TASK_DUE_COLUMN_NAME}, " +
                    $"{TaskDTO.TASK_CREATE_COLUMN_NAME}, {TaskDTO.TASK_ID_COLUMN_NAME}, {TaskDTO.TASK_BOARD_ID_COLUMN_NAME}) " +
                    "VALUES (@Title, @Description, @DueDate, @CreationTime, @Id, @BoardId);";

                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@DueDate", task.Due.ToString("o"));
                command.Parameters.AddWithValue("@CreationTime", task.CreationTime.ToString("o"));
                command.Parameters.AddWithValue("@Id", task.Id);
                command.Parameters.AddWithValue("@BoardId", task.BoardId);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error("Insert failed for task.", ex);
                    return false;
                }
            }
        }

        internal bool Update(int taskId, object newValue, string column)
        {
            // just for now an inital check of newValue's type
            if (newValue.GetType() == taskId.GetType())
                newValue = (int)newValue;
            else newValue = (string)newValue;
                using (SqliteConnection connection = new SqliteConnection(_connectionString))
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE {TASK_TABLE_NAME} SET {column} = @Value WHERE {TaskDTO.TASK_ID_COLUMN_NAME} = @Id;";
                    command.Parameters.AddWithValue("@Value", newValue);
                    command.Parameters.AddWithValue("@Id", taskId);

                    try
                    {
                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Update failed for task id {taskId}.", ex);
                        return false;
                    }
                }
        }

        internal bool Delete(TaskDTO task)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM {TASK_TABLE_NAME} WHERE {TaskDTO.TASK_ID_COLUMN_NAME} = @Id;";
                command.Parameters.AddWithValue("@Id", task.Id);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Log.Error($"Delete failed for task id {task.Id}.", ex);
                    return false;
                }
            }
        }
    }
}