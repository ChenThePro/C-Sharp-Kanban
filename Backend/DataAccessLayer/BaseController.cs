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
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly string _connectionString;
        protected readonly string TableName;

        protected BaseController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            _connectionString = $"Data Source={path}; Version=3;";
            TableName = tableName;
        }

        internal bool Insert(TDTO dto)
        {
            var columns = dto.GetColumnNames();
            var values = dto.GetColumnValues();
            var columnList = string.Join(", ", columns);
            var paramList = string.Join(", ", columns.Select(c => $"@{c}"));
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {TableName} ({columnList}) VALUES ({paramList});";
            for (int i = 0; i < columns.Length; i++)
                command.Parameters.AddWithValue($"@{columns[i]}", values[i]);
            try
            {
                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    Log.Info($"Insert succeeded for table {TableName}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Insert failed for table {TableName}.", ex);
                return false;
            }
        }

        internal bool Delete(string keyColumn, object key)
        {
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {TableName} WHERE {keyColumn} = @Key;";
            command.Parameters.AddWithValue("@Key", key);
            try
            {
                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    Log.Info($"Delete succeeded on {TableName} for key {key}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Delete failed on {TableName} for key {key}.", ex);
                return false;
            }
        }

        internal bool Update(string keyColumn, object key, string column, object newValue)
        {
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"UPDATE {TableName} SET {column} = @Value WHERE {keyColumn} = @Key;";
            command.Parameters.AddWithValue("@Value", newValue);
            command.Parameters.AddWithValue("@Key", key);
            try
            {
                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    Log.Info($"Update succeeded on {TableName} for key {key}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Update failed on {TableName} for key {key}.", ex);
                return false;
            }
        }

        internal List<TDTO> SelectAll()
        {
            List<TDTO> results = new();
            using var connection = new SqliteConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {TableName};";
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                    results.Add(ConvertReaderToDTO(reader));
            }
            catch (Exception ex)
            {
                Log.Error($"Select failed on {TableName}.", ex);
            }
            Log.Info($"Select succeeded on {TableName}.");
            return results;
        }

        protected abstract TDTO ConvertReaderToDTO(SqliteDataReader reader);
    }
}