namespace TweetishApp.Core.Entities
{
    public class Following
    {
        public int Id {get; set;}
        public string FollowerId {get; set;}
        public string FolloweeId {get; set;}
    }
}