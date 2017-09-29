namespace AlertCenter
{
    public static class ErrorMessages
    {
        public const string EmailWasNotSet = "Email was not set";
        public const string Expired = "Expired, please reauthenticate";
        public const string TopicDoesNotExist = "That topic doesn't exist, did you mistype the topic?";
        public const string AlreadySubscribed = "You already subscribed to that topic";
        public const string NotSubscribed = "You have not subscribed to that topic, did you mistype the topic?";
        public const string TopicExists = "Topic already exists";
        public const string InvalidJsonOrIncorrectType = "Either the body wasn't json or it was the incorrect type";
        public const string InvalidEmail = "The email address is misformatted";
    }
}
