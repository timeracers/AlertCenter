using System.Linq;
using AlertCenter.Exceptions;
using System.Data.Common;
using System;
using MimeKit;
using AlertCenter.Database;
using AlertCenter.Middleware;

namespace AlertCenter.Emails
{
    public class EmailGateway
    {
        private SqlDatabase _database;

        public EmailGateway(SqlDatabase database)
        {
            _database = database;
        }

        public JsonContent Get(Guid userId)
        {
            var email = _database.Query<string>($"SELECT email FROM alert.\"Emails\" WHERE userId = '{userId}'").ToArray();
            if (email.Count() == 0)
                return new JsonFailure(new DoesNotExistException(ExceptionMessages.EmailWasNotSet));
            return new JsonSuccess(email.First());
        }

        public JsonContent Set(Guid userId, string email, string username)
        {
            try
            {
                new MailboxAddress(email);
            }
            catch
            {
                return new JsonFailure(new InvalidParametersException(ExceptionMessages.InvalidEmail));
            }
            _database.Execute($"INSERT INTO alert.\"Emails\" (userId, username, email) VALUES ('{userId}', '{username}', '{email}') " +
                $"ON CONFLICT (userId) DO UPDATE SET (email) = ('{email}')");
            return new JsonSuccess();
        }

        public JsonContent Delete(Guid userId)
        {
            try
            {
                _database.Execute($"DELETE FROM alert.\"Emails\" WHERE userId = '{userId}'");
                return new JsonSuccess();
            }
            catch (DbException x)
            {
                if (x.ErrorNumber() == 23503)
                    return new JsonFailure(new ResourceRequiredException());
                throw x;
            }
        }
    }
}
