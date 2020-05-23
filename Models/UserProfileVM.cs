using TweetishApp.Core.Entities;
using System.Collections.Generic;

namespace TweetishApp.Models
{
    public class UserProfileVM
    {
        public string UserId {get; set;}
        public string Nickname {get; set;}
        public List<Tweet> Tweets {get; set;} = new List<Tweet>();

        public UserProfileVM(string userId, string nickname, List<Tweet> tweets)
        {
            UserId = userId;
            Nickname = nickname;
            Tweets = tweets;
        }
    }
}