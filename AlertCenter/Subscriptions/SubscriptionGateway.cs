using System.Linq;
using AlertCenter.Exceptions;
using System.Data.Common;
using System;
using AlertCenter.Database;
using AlertCenter.Middleware;

namespace AlertCenter.Subscriptions
{
    public class SubscriptionGateway
    {
        private SqlDatabase _database;

        public SubscriptionGateway(SqlDatabase database)
        {
            _database = database;
        }

        public JsonContent GetAll(Guid userId)
        {
            return new JsonSuccess(_database.Query<string>($"SELECT topic FROM alert.\"Subscriptions\" WHERE userId = '{userId}'"));
        }

        public JsonContent Add(Guid userId, string topic)
        {
            try
            {
                _database.Execute($"INSERT INTO alert.\"Subscriptions\" (userId, topic) VALUES ('{userId}', '{topic}')");
                return new JsonSuccess();
            }
            catch (DbException x)
            {
                if (x.Message == "23503: insert or update on table \"Subscriptions\" violates foreign key constraint \"Subscriptions_userid_fkey\"")
                    return new JsonFailure(new DoesNotExistException(ExceptionMessages.EmailWasNotSet));
                if (x.ErrorNumber() == 23503)
                    return new JsonFailure(new DoesNotExistException(ExceptionMessages.TopicDoesNotExist));
                if (x.ErrorNumber() == 23505)
                    return new JsonFailure(new AlreadyExistsException(ExceptionMessages.AlreadySubscribed));
                throw x;
            }
        }

        public JsonContent Remove(Guid userId, string topic)
        {
            if (!_database.Query<bool>($"SELECT EXISTS(SELECT 1 FROM alert.\"Subscriptions\" WHERE userId = '{userId}' AND topic = '{topic}')")
                .First())
                    return new JsonFailure(new DoesNotExistException(ExceptionMessages.NotSubscribed));
            _database.Execute($"DELETE FROM alert.\"Subscriptions\" WHERE userId = '{userId}' AND topic = '{topic}'");
            return new JsonSuccess();
        }

        public JsonContent RemoveAll(Guid userId)
        {
            _database.Execute($"DELETE FROM alert.\"Subscriptions\" WHERE userId = '{userId}'");
            return new JsonSuccess();
        }
    }
}
