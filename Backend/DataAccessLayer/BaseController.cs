using log4net;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class BaseController<TDTO> where TDTO : IDTO
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly string _connectionString;
        protected readonly string _tableName;

        protected BaseController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            _connectionString = $"Data Source={path};";
            _tableName = tableName;
        }

        internal bool Insert(TDTO dto)
        {
            var columns = dto.GetColumnNames();
            var values = dto.GetColumnValues();
            var columnList = string.Join(", ", columns);
            var paramList = string.Join(", ", columns.Select(c => $"@{c}"));
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {_tableName} ({columnList}) VALUES ({paramList});";
            for (int i = 0; i < columns.Length; i++)
                command.Parameters.AddWithValue($"@{columns[i]}", values[i]);
            try
            {
                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    Log.Info($"Insert succeeded for table {_tableName}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Insert failed for table {_tableName}.", ex);
                return false;
            }
        }

        internal bool Delete(string keyColumn, object key)
        {
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {_tableName} WHERE {keyColumn} = @Key;";
            command.Parameters.AddWithValue("@Key", key);
            try
            {
                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    Log.Info($"Delete succeeded on {_tableName} for key {key}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Delete failed on {_tableName} for key {key}.", ex);
                return false;
            }
        }

        internal bool Update(string keyColumn, object key, string column, object newValue)
        {
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"UPDATE {_tableName} SET {column} = @Value WHERE {keyColumn} = @Key;";
            command.Parameters.AddWithValue("@Value", newValue);
            command.Parameters.AddWithValue("@Key", key);
            try
            {
                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    Log.Info($"Update succeeded on {_tableName} for key {key}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Update failed on {_tableName} for key {key}.", ex);
                return false;
            }
        }

        internal List<TDTO> SelectAll()
        {
            List<TDTO> results = new();
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {_tableName};";
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                    results.Add(ConvertReaderToDTO(reader));
            }
            catch (Exception ex)
            {
                Log.Error($"Select failed on {_tableName}.", ex);
            }
            Log.Info($"Select succeeded on {_tableName}.");
            return results;
        }

        internal bool DeleteAll()
        {
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {_tableName};";
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                Log.Info($"DeleteAll succeeded on {_tableName}. {rowsAffected} rows deleted.");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"DeleteAll failed on {_tableName}.", ex);
                return false;
            }
        }

        internal bool DeleteAllAndResetAutoIncrement()
        {
            using var connection = new SqliteConnection(_connectionString);
            try
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();

                using var deleteCommand = connection.CreateCommand();
                deleteCommand.CommandText = $"DELETE FROM {_tableName};";
                deleteCommand.Transaction = transaction;
                int rowsAffected = deleteCommand.ExecuteNonQuery();

                using var resetCommand = connection.CreateCommand();
                resetCommand.CommandText = $"DELETE FROM sqlite_sequence WHERE name = '{_tableName}';";
                resetCommand.Transaction = transaction;
                resetCommand.ExecuteNonQuery();

                transaction.Commit();
                Log.Info($"DeleteAllAndResetAutoIncrement succeeded on {_tableName}. {rowsAffected} rows deleted and auto-increment reset.");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"DeleteAllAndResetAutoIncrement failed on {_tableName}.", ex);
                return false;
            }
        }

        protected abstract TDTO ConvertReaderToDTO(SqliteDataReader reader);
    }
}