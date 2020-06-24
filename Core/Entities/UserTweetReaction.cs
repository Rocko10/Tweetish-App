using TweetishApp.Data;

namespace TweetishApp.Core.Entities
{
    public class UserTweetReaction
    {
        public int Id {get; set;}
        public string UserId {get; set;}
        public AppUser User {get; set;}

        public int TweetId {get; set;}
        public Tweet Tweet {get; set;} 

        public int ReactionId {get; set;}
        public Reaction Reaction {get; set;}
        public bool Reacted {get; set;} = false;
    }
}