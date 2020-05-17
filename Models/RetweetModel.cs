using TweetishApp.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetishApp.Models
{
    [Table("retweets")]
    public class RetweetModel
    {
        public int Id {get; set;}

        public string UserId {get; set;}
        public AppUser User {get; set;}

        public int TweetId {get; set;}
        public TweetModel Tweet {get; set;}
    }
}