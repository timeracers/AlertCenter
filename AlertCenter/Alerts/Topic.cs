﻿namespace AlertCenter.Alerts
{
    public class Topic
    {
        public string Name { get; set; }

        private Topic() { }

        public Topic(string name)
        {
            Name = name;
        }
    }
}
