using TweetishApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TweetishApp.Core.Interfaces
{
    public interface IFollowingService
    {
        Task<List<Following>> GetAllFollowersOf(string userId);
        Task<List<Following>> GetAllFolloweesFrom(string userId);
        Task<Following> Create(Following folowing);
        Task<List<Tweet>> GetFolloweesTweets(string userId);
        Task Remove(Following following);
    }
}