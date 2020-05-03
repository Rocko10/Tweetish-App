using System.Collections.Generic;

namespace TweetishApp.Models
{
    public class FollowerModel
    {
        public string Id {get; set;}

        public List<FollowingModel> Followings {get; set;} = new List<FollowingModel>();
    }
}