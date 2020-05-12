using Microsoft.AspNetCore.Identity;
using TweetishApp.Models;
using System.Collections.Generic;

namespace TweetishApp.Data
{
    public class AppUser : IdentityUser
    {
        public string Nickname {get; set;}
        public List<TweetModel> Tweets {get; set;}
        public List<FollowingModel> Followings {get; set;}
        public List<RetweetModel> Retweets {get; set;} = new List<RetweetModel>();
        // public List<ReactionModel> Reactions {get; set;} = new List<ReactionModel>();
    }
}