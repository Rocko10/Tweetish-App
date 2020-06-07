using TweetishApp.Data;

namespace TweetishApp.Core.Entities
{
    public class Retweet
    {
        public int Id {get; set;}
        // Person who retweets
        public string UserId {get; set;}
        public AppUser User {get; set;}
        public int TweetId {get; set;}
        public Tweet Tweet {get; set;}
    }
}