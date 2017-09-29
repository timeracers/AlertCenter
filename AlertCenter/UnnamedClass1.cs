using System.Linq;
using AlertCenter.Controllers;
using AlertCenter.Exceptions;
using System.Data.Common;
using System;
using MimeKit;

namespace AlertCenter
{
    public class UnnamedClass1
    {
        private SqlDatabase _database;

        public UnnamedClass1(SqlDatabase database)
        {
            _database = database;
        }

        public JsonStatusCode Get(Guid userId)
        {
            var email = _database.Query<string>($"SELECT email FROM alert.\"Emails\" WHERE userId = '{userId}'").ToArray();
            if (email.Count() == 0)
                return new JsonHttpException(new DoesNotExistException(ErrorMessages.EmailWasNotSet));
            return new JsonContent(email.First());
        }

        public JsonStatusCode Set(Guid userId, string email, string username)
        {
            try
            {
                new MailboxAddress(email);
            }
            catch
            {
                return new JsonHttpException(new InvalidParametersException(ErrorMessages.InvalidEmail));
            }
            _database.Execute($"INSERT INTO alert.\"Emails\" (userId, username, email) VALUES ('{userId}', '{username}', '{email}') " +
                $"ON CONFLICT (userId) DO UPDATE SET (email) = ('{email}')");
            return new JsonNoContent();
        }

        public JsonStatusCode Delete(Guid userId)
        {
            try
            {
                _database.Execute($"DELETE FROM alert.\"Emails\" WHERE userId = '{userId}'");
                return new JsonNoContent();
            }
            catch (DbException x)
            {
                if (x.Message.Substring(0, x.Message.IndexOf(":")) == "23503")
                    return new JsonHttpException(new ResourceRequiredException());
                throw x;
            }
        }
    }
}
