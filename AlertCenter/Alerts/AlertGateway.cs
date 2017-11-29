using System;
using System.Linq;
using AlertCenter.Exceptions;
using System.Data.Common;
using System.Threading;
using AlertCenter.Emails;
using AlertCenter.Database;
using AlertCenter.Middleware;

namespace AlertCenter.Alerts
{
    public class AlertGateway
    {
        private SqlDatabase _database;
        private EmailClient _email;

        public AlertGateway(SqlDatabase database, EmailClient email)
        {
            _database = database;
            _email = email;
        }

        public JsonContent GetTopics()
        {
            return new JsonSuccess(_database.Query<Topic>("SELECT * FROM alert.\"Topics\""));
        }

        public JsonContent GetAllFrom(string topic)
        {
            var alerts = _database.Query<Alert>($"SELECT * FROM alert.\"Alerts\" WHERE topic = '{topic}'");
            if (alerts.Count() == 0 && !_database.Query<bool>($"SELECT EXISTS(SELECT 1 FROM alert.\"Topics\" WHERE name = '{topic}')").First())
                return new JsonFailure(new DoesNotExistException(ExceptionMessages.TopicDoesNotExist));
            return new JsonSuccess(alerts);
        }

        public JsonContent AddTo(string topic, Guid userId, string username, string message)
        {
            try
            {
                _database.Execute($"INSERT INTO alert.\"Alerts\" (topic, userId, username, message, unixEpoch) VALUES " +
                    $"('{topic}', '{userId}', '{username}', '{message}', '{DateTimeOffset.Now.ToUnixTimeSeconds()}')");
                new Thread(new ThreadStart(() => EmailSubscribers(topic, message))).Start();
                return new JsonSuccess();
            }
            catch (DbException x)
            {
                if (x.ErrorNumber() == 23503)
                    return new JsonFailure(new DoesNotExistException(ExceptionMessages.TopicDoesNotExist));
                throw x;
            }
        }

        private void EmailSubscribers(string topic, string message)
        {
            var subscribers = _database.Query<Guid>($"SELECT userid FROM alert.\"Subscriptions\" WHERE topic = '{topic}'");
            if (subscribers.Count() != 0)
            {
                var emailAddresses = _database.Query<string>($"SELECT email FROM alert.\"Emails\" WHERE userId IN " +
                    $"('{string.Join("', '", subscribers)}')");
                _email.EmailAllByBcc(emailAddresses, topic, message);
            }
        }

        public JsonContent AddTopic(string topic)
        {
            try
            {
                _database.Execute($"INSERT INTO alert.\"Topics\" (topic) VALUES ('{topic}')");
                return new JsonSuccess();
            }
            catch (DbException x)
            {
                if (x.ErrorNumber() == 23505)
                    return new JsonFailure(new AlreadyExistsException(ExceptionMessages.TopicExists));
                throw x;
            }
        }
    }
}
