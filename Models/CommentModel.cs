using System.ComponentModel.DataAnnotations.Schema;

namespace TweetishApp.Models
{
    [Table("comments")]
    public class CommentModel
    {
        public int Id {get; set;}
        public string Text {get; set;}
        public int TweetId {get; set;}
        public TweetModel Tweet {get; set;}
    }
}