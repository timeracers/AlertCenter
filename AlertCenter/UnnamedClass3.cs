using System;
using System.Linq;
using AlertCenter.Controllers;
using AlertCenter.Dtos;
using AlertCenter.Exceptions;
using System.Data.Common;

namespace AlertCenter
{
    public class UnnamedClass3
    {
        private SqlDatabase _database;

        public UnnamedClass3(SqlDatabase database)
        {
            _database = database;
        }

        public JsonStatusCode GetTopics()
        {
            return new JsonContent(_database.Query<Topic>("SELECT * FROM alert.\"Topics\"").ToArray());
        }

        public JsonStatusCode GetAllFrom(string topic)
        {
            var alerts = _database.Query<Alert>($"SELECT * FROM alert.\"Alerts\" WHERE topic = '{topic}'").ToArray();
            if (alerts.Count() == 0 && _database.Query<bool>($"SELECT EXISTS(SELECT 1 FROM alert.\"Topics\" WHERE topic = '{topic}')").First())
                return new JsonHttpException(new DoesNotExistException(ErrorMessages.TopicDoesNotExist));
            return new JsonContent(alerts);
        }

        public JsonStatusCode AddTo(string topic, string userId, string message)
        {
            try
            {
                _database.Execute($"INSERT INTO alert.\"Alerts\" (topic, userId, message, unixEpoch) VALUES " +
                    $"('{topic}', '{userId}', '{message}', '{DateTimeOffset.Now.ToUnixTimeSeconds()}')");
                //Insert Email Logic;
                return new JsonNoContent();
            }
            catch (DbException x)
            {
                if (x.Message == "Magic String Foreign Violation")
                    return new JsonHttpException(new DoesNotExistException(ErrorMessages.TopicDoesNotExist));
                throw x;
            }
        }

        public JsonStatusCode AddTopic(string topic)
        {
            try
            {
                _database.Execute($"INSERT INTO alert.\"Topics\" (topic) VALUES ('{topic}')");
                return new JsonNoContent();
            }
            catch (DbException x)
            {
                if (x.Message.Substring(0, x.Message.IndexOf(":")) == "23505")
                    return new JsonHttpException(new AlreadyExistsException(ErrorMessages.TopicExists));
                throw x;
            }
        }
    }
}
