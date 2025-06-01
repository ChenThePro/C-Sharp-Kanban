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
            string dbPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            _connectionString = $"Data Source={dbPath};";
            _tableName = tableName;
        }

        internal void Insert(TDTO dto)
        {
            string[] columns = dto.GetColumnNames();
            object[] values = dto.GetColumnValues();
            string columnList = string.Join(", ", columns);
            string paramList = string.Join(", ", columns.Select(c => $"@{c}"));
            ExecuteQuery(
                $"INSERT INTO {_tableName} ({columnList}) VALUES ({paramList});",
                command =>
                {
                    for (int i = 0; i < columns.Length; i++)
                        command.Parameters.AddWithValue($"@{columns[i]}", values[i] ?? DBNull.Value);
                },
                "Insert");
        }

        internal List<TDTO> SelectAll()
        {
            List<TDTO> results = new();
            using SqliteConnection connection = new(_connectionString);
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {_tableName};";
            connection.Open();
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                results.Add(ConvertReaderToDTO(reader));
            Log.Info($"SelectAll succeeded on {_tableName}, {results.Count} records loaded.");
            return results;
        }

        internal void DeleteAll() =>
            ExecuteQuery(
                $"DELETE FROM {_tableName};",
                command => { },
                "DeleteAll");

        internal void DeleteAllAndResetAutoIncrement()
        {
            using SqliteConnection connection = new(_connectionString);
            connection.Open();
            using SqliteTransaction transaction = connection.BeginTransaction();
            using SqliteCommand deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = $"DELETE FROM {_tableName};";
            deleteCommand.Transaction = transaction;
            int rows = deleteCommand.ExecuteNonQuery();
            using SqliteCommand resetCommand = connection.CreateCommand();
            resetCommand.CommandText = $"DELETE FROM sqlite_sequence WHERE name = '{_tableName}';";
            resetCommand.Transaction = transaction;
            resetCommand.ExecuteNonQuery();
            transaction.Commit();
            Log.Info($"All rows deleted and autoincrement reset on {_tableName}. Rows affected: {rows}");
        }

        protected void ExecuteQuery(string sql, Action<SqliteCommand> parameterAction, string operationName)
        {
            using SqliteConnection connection = new(_connectionString);
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = sql;
            parameterAction(command);
            connection.Open();
            int result = command.ExecuteNonQuery();
            if (result > 0)
                Log.Info($"{operationName} succeeded on {_tableName}. Rows affected: {result}");
            else Log.Warn($"{operationName} on {_tableName} affected 0 rows");
        }

        protected abstract TDTO ConvertReaderToDTO(SqliteDataReader reader);
    }
}