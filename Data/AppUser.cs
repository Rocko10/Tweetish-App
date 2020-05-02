using Microsoft.AspNetCore.Identity;
using TweetishApp.Models;
using System.Collections.Generic;

namespace TweetishApp.Data
{
    public class AppUser : IdentityUser
    {
        public string Nickname {get; set;}
        public List<TweetModel> Tweets {get; set;}
    }
}