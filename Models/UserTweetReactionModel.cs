using TweetishApp.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetishApp.Models
{
    [Table("user_tweet_reactions")]
    public class UserTweetReactionModel
    {
        public int Id {get; set;}
        public string UserId {get; set;}
        public AppUser User {get; set;}

        public int TweetId {get; set;}
        public TweetModel Tweet {get; set;}

        public int ReactionId {get; set;}
        public ReactionModel Reaction {get; set;}
    }
}