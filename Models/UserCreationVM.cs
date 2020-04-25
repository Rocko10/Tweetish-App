using System.ComponentModel.DataAnnotations;

namespace TweetishApp.Models
{
    public class UserCreationVM
    {
        [Required]
        public string Nickname {get; set;}
        [Required]
        public string Password {get; set;}
        public string ConfirmPassword {get; set;}
    }
}