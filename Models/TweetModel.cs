using System;
using System.ComponentModel.DataAnnotations.Schema;
using TweetishApp.Data;

namespace TweetishApp.Models
{
    [Table("tweets")]
    public class TweetModel
    {
        public int Id {get; set;}
        public string UserId {get; set;}
        public AppUser User {get; set;}
        public string Text {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}