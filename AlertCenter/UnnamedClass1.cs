using System.Linq;
using AlertCenter.Controllers;
using AlertCenter.Dtos;
using AlertCenter.Exceptions;
using Npgsql;
using System.Data.Common;

namespace AlertCenter
{
    public class UnnamedClass1
    {
        private SqlDatabase _database;

        public UnnamedClass1(SqlDatabase database)
        {
            _database = database;
        }

        public JsonStatusCode Get(string userId)
        {
            var email = _database.Query<string>($"SELECT email FROM alert.\"Emails\" WHERE userId = '{userId}'").ToArray();
            if (email.Count() == 0)
                return new JsonHttpException(new DoesNotExistException(ErrorMessages.EmailWasNotSet));
            return new JsonContent(email.First());
        }

        public JsonStatusCode Set(string userId, string email)
        {
            _database.Execute($"INSERT INTO alert.\"Emails\" (userId, email) VALUES ('{userId}', '{email}') ON CONFLICT (userId) DO UPDATE SET " +
                $"(email) = ('{email}')");
            return new JsonNoContent();
        }

        public JsonStatusCode Delete(string userId)
        {
            try
            {
                _database.Execute($"DELETE FROM alert.\"Emails\" WHERE userId = '{userId}'");
                return new JsonNoContent();
            }
            catch (DbException x)
            {
                if (x.Message == "Magic String used as foreign key")
                    return new JsonHttpException(new ResourceRequiredException());
                throw x;
            }
        }
    }
}
