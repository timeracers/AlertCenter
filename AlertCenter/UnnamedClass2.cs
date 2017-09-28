using System.Linq;
using AlertCenter.Controllers;
using AlertCenter.Exceptions;
using System.Data.Common;

namespace AlertCenter
{
    public class UnnamedClass2
    {
        private SqlDatabase _database;

        public UnnamedClass2(SqlDatabase database)
        {
            _database = database;
        }

        public JsonStatusCode GetAll(string userId)
        {
            var subscribedTopics = _database.Query<string>($"SELECT topic FROM alert.\"Subscriptions\" WHERE userId = '{userId}'").ToArray();
            return new JsonContent(subscribedTopics);
        }

        public JsonStatusCode Add(string userId, string topic)
        {
            try
            {
                _database.Execute($"INSERT INTO alert.\"Subscriptions\" (userId, topic) VALUES ('{userId}', '{topic}')");
                return new JsonNoContent();
            }
            catch (DbException x)
            {
                if (x.Message == "Magic String foreign violation")
                    return new JsonHttpException(new DoesNotExistException(ErrorMessages.EmailWasNotSet));
                if (x.Message == "Magic String foreign violation")
                    return new JsonHttpException(new DoesNotExistException(ErrorMessages.TopicDoesNotExist));
                if (x.Message == "Magic String Already Exists")
                    return new JsonHttpException(new AlreadyExistsException(ErrorMessages.AlreadySubscribed));
                throw x;
            }
        }

        public JsonStatusCode Remove(string userId, string topic)
        {
            if(_database.Query<bool>($"SELECT EXISTS(SELECT 1 FROM alert.\"Subscriptions\" WHERE userId = '{userId}' AND topic = '{topic}')").First())
                return new JsonHttpException(new DoesNotExistException(ErrorMessages.NotSubscribed));
            _database.Execute($"DELETE FROM alert.\"Subscriptions\" WHERE userId = '{userId}' AND topic = '{topic}'");
            return new JsonNoContent();
        }

        public JsonStatusCode RemoveAll(string userId)
        {
            _database.Execute($"DELETE FROM alert.\"Subscriptions\" WHERE userId = '{userId}'");
            return new JsonNoContent();
        }
    }
}
