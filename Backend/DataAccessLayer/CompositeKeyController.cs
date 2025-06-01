using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class CompositeKeyController<TDTO> : BaseController<TDTO> where TDTO : IDTO
    {
        internal CompositeKeyController(string tableName) : base(tableName) { }

        internal void Delete(string keyColumn1, object key1, string keyColumn2, object key2) =>
            ExecuteQuery(
                $"DELETE FROM {_tableName} WHERE {keyColumn1} = @Key1 AND {keyColumn2} = @Key2;",
                command =>
                {
                    command.Parameters.AddWithValue("@Key1", key1);
                    command.Parameters.AddWithValue("@Key2", key2);
                },
                $"Delete where {keyColumn1}={key1} AND {keyColumn2}={key2}");

        internal void Update(string keyColumn1, object key1, string keyColumn2, object key2, string column, object newValue) =>
            ExecuteQuery(
                $"UPDATE {_tableName} SET {column} = @Value WHERE {keyColumn1} = @Key1 AND {keyColumn2} = @Key2;",
                command =>
                {
                    command.Parameters.AddWithValue("@Value", newValue ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Key1", key1);
                    command.Parameters.AddWithValue("@Key2", key2);
                },
                $"Update {column}={newValue} where {keyColumn1}={key1} AND {keyColumn2}={key2}");
    }
}
