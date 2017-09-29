using System;
using System.Linq;
using AlertCenter.Controllers;
using AlertCenter.Dtos;
using AlertCenter.Exceptions;
using System.Data.Common;
using System.Collections.Generic;
using MimeKit;
using System.Text;
using MailKit.Net.Smtp;
using System.Threading;

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
            if (alerts.Count() == 0 && !_database.Query<bool>($"SELECT EXISTS(SELECT 1 FROM alert.\"Topics\" WHERE topic = '{topic}')").First())
                return new JsonHttpException(new DoesNotExistException(ErrorMessages.TopicDoesNotExist));
            return new JsonContent(alerts);
        }

        public JsonStatusCode AddTo(string topic, Guid userId, string username, string message)
        {
            try
            {
                _database.Execute($"INSERT INTO alert.\"Alerts\" (topic, userId, username, message, unixEpoch) VALUES " +
                    $"('{topic}', '{userId}', '{username}', '{message}', '{DateTimeOffset.Now.ToUnixTimeSeconds()}')");

                var emailThread = new Thread(new ThreadStart(() => EmailSubscribers(topic, message)));
                emailThread.Start();

                return new JsonNoContent();
            }
            catch (DbException x)
            {
                if (x.Message.Substring(0, x.Message.IndexOf(":")) == "23503")
                    return new JsonHttpException(new DoesNotExistException(ErrorMessages.TopicDoesNotExist));
                throw x;
            }
        }

        private void EmailSubscribers(string topic, string message)
        {
            var subscribers = _database.Query<Guid>($"SELECT userid FROM alert.\"Subscriptions\" WHERE topic = '{topic}'");
            if (subscribers.Count() != 0)
            {
                var emailContacts = _database.Query<UserEmail>($"SELECT email, username FROM alert.\"Emails\" WHERE userId IN " +
                    $"('{string.Join("', '", subscribers)}')");
                EmailAll(emailContacts, topic, message);
            }
        }

        private void EmailAll(IEnumerable<UserEmail> emailContacts, string topic, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(Config.EmailName, Config.EmailAddress));
            mail.Bcc.AddRange(emailContacts.Select(contact => new MailboxAddress(contact.Username, contact.Email)));
            mail.Subject = topic;
            mail.Body = new TextPart("plain") { Text = message };
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(Config.EmailService);
                Config.EmailAuthenticate(client);
                client.Send(mail);
                client.Disconnect(true);
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
