namespace TweetishApp.Core.Entities
{
    public class Retweet
    {
        // Person who retweets
        public int Id {get; set;}
        public string UserId {get; set;}
        public int TweetId {get; set;}
    }
}