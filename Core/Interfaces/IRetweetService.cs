using System.Threading.Tasks;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Interfaces
{
    public interface IRetweetService
    {
        Task<Retweet> Create(Retweet retweet);
        Task Remove(Retweet retweet);
        Task Toggle(Retweet retweet);
        Task<Retweet> GetInfo(Retweet retweet);
    }
}