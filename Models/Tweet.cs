using System;
using TweetishApp.Data;

namespace TweetishApp.Models
{
    public class Tweet
    {
        public int Id {get; set;}
        public string UserId {get; set;}
        public AppUser User {get; set;}
        public string Text {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}