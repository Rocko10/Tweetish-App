using Microsoft.AspNetCore.Identity;

namespace TweetishApp.Data
{
    public class AppUser : IdentityUser
    {
        public string Nickname {get; set;}
    }
}