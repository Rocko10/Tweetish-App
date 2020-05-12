namespace TweetishApp.Core.Entities
{
    public class Reaction
    {
        public int Id {get; set;}
        public string Name {get; set;}

        public Reaction(string name)
        {
            Name = name;
        }
    }
}