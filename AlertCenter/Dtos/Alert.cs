using System;

namespace AlertCenter.Dtos
{
    public class Alert
    {
        public string Topic { get; set; }
        public string Message { get; set; }
        public long UnixEpoch { get; set; }
        public string Username { get; set; }
        public Guid UserId { get; set; }

        private Alert() { }

        public Alert(string topic, string message, long unixEpoch, string username, Guid userId)
        {
            Topic = topic;
            Message = message;
            UnixEpoch = unixEpoch;
            Username = username;
            UserId = UserId;
        }
    }
}
