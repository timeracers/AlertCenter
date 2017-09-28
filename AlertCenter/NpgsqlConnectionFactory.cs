using Npgsql;
using System.Data;

namespace AlertCenter
{
    public class NpgsqlConnectionFactory
    {
        private string _connectionString;

        public NpgsqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
