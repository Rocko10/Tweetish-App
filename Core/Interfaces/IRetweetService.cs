using System.Threading.Tasks;
using TweetishApp.Core.Entities;
using System.Collections.Generic;

namespace TweetishApp.Core.Interfaces
{
    public interface IRetweetService
    {
        Task<Retweet> Create(Retweet retweet);
        Task Remove(Retweet retweet);
        Task Toggle(Retweet retweet);
        Task<Retweet> GetInfo(Retweet retweet);
        Task<List<Tweet>> GetRetweetsByUserId(string userId);
        Task<bool> IsRetweeted(string userId, int tweetId);
    }
}