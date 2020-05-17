using System.ComponentModel.DataAnnotations.Schema;

namespace TweetishApp.Models
{
    [Table("reactions")]
    public class ReactionModel
    {
        public int Id {get; set;}
        public string Name {get; set;}
    }
}