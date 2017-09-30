using System;

namespace AlertCenter.Subscriptions
{
    public class Subscription
    {
        public string Topic { get; set; }
        public Guid UserId { get; set; }

        private Subscription() {}

        public Subscription(string topic, Guid userId)
        {
            Topic = topic;
            UserId = userId;
        }
    }
}
