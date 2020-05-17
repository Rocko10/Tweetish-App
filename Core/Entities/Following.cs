using TweetishApp.Data;

namespace TweetishApp.Core.Entities
{
    public class Following
    {
        public int Id {get; set;}
        public AppUser Follower {get; set;}
        public string FollowerId {get; set;}
        public AppUser Followee {get; set;}
        public string FolloweeId {get; set;}
    }
}