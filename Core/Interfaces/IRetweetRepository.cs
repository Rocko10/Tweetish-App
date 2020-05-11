using TweetishApp.Core.Entities;
using System.Threading.Tasks;

namespace TweetishApp.Core.Interfaces
{
    public interface IRetweetRepository
    {
        Task<Retweet> Create(Retweet retweet);
        Task Remove(Retweet retweet);
        Task Toggle(Retweet retweet);
        Task<Retweet> GetInfo(Retweet retweet);
    }
}