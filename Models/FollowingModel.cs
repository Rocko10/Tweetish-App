using System.ComponentModel.DataAnnotations.Schema;
using TweetishApp.Data;

namespace TweetishApp.Models
{
    [Table("followings")]
    public class FollowingModel
    {
        public int Id {get; set;}
        
        public string FollowerId {get; set;}
        public AppUser Follower {get; set;}

        public string FolloweeId {get; set;}
        public AppUser Followee {get; set;}
    }
}