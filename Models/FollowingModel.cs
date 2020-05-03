using System.ComponentModel.DataAnnotations.Schema;

namespace TweetishApp.Models
{
    [Table("followings")]
    public class FollowingModel
    {
        public int Id {get; set;}
        
        public string FollowerId {get; set;}
        public FollowerModel Follower {get; set;}

        public string FolloweeId {get; set;}
        public FolloweeModel Followee {get; set;}
    }
}