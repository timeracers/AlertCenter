namespace AlertCenter.Dtos
{
    public class Subscription
    {
        public string Topic { get; set; }
        public string UserId { get; set; }

        public Subscription(string topic, string userId)
        {
            Topic = topic;
            UserId = userId;
        }
    }
}
