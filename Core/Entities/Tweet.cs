using System;

namespace TweetishApp.Core.Entities
{
    public class Tweet
    {
        public int Id {get; set;}
        public string Text {get; set;}
        public string UserId {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}

        public Tweet(string userId)
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Tweet() {}
    }
}