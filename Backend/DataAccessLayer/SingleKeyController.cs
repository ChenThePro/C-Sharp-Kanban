using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class SingleKeyController<TDTO> : BaseController<TDTO> where TDTO : IDTO
    {
        internal SingleKeyController(string tableName) : base(tableName) { }

        internal void Delete(string keyColumn, object key) =>
            ExecuteQuery(
                $"DELETE FROM {_tableName} WHERE {keyColumn} = @Key;",
                command => command.Parameters.AddWithValue("@Key", key),
                $"Delete where {keyColumn}={key}");

        internal void Update(string keyColumn, object key, string column, object newValue) =>
            ExecuteQuery(
                $"UPDATE {_tableName} SET {column} = @Value WHERE {keyColumn} = @Key;",
                command =>
                {
                    command.Parameters.AddWithValue("@Value", newValue ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Key", key);
                },
                $"Update {column}={newValue} where {keyColumn}={key}");
    }
}
