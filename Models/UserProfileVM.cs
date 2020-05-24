namespace TweetishApp.Models
{
    public class UserProfileVM
    {
        public string UserId {get; set;}
        public string Nickname {get; set;}

        public UserProfileVM(string userId, string nickname)
        {
            UserId = userId;
            Nickname = nickname;
        }
    }
}