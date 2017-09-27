namespace AlertCenter.Dtos
{
    public class Alert
    {
        public string Topic { get; set; }
        public string Message { get; set; }
        public long UnixEpoch { get; set; }
        public string UserId { get; set; }

        public Alert(string topic, string message, long unixEpoch, string userId)
        {
            Topic = topic;
            Message = message;
            UnixEpoch = unixEpoch;
            UserId = UserId;
        }
    }
}
