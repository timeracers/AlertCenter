using Dapper;
using System.Collections.Generic;
using System.Data;

namespace AlertCenter
{
    public class SqlDatabase
    {
        private NpgsqlConnectionFactory _connectionFactory;

        public SqlDatabase(NpgsqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Execute(string sql)
        {
            using (IDbConnection c = _connectionFactory.Create())
                return c.Execute(sql);
        }

        public IEnumerable<T> Query<T>(string sql)
        {
            using (IDbConnection c = _connectionFactory.Create())
                return c.Query<T>(sql);
        }
    }
}
