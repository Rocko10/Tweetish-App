namespace TweetishApp.Core.Entities
{
    public class Comment
    {
        public int Id {get; set;}
        public string Text {get; set;}
        public int TweetId {get; set;}
        public Tweet Tweet {get; set;}
    }
}