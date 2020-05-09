using System.Threading.Tasks;
using System.Collections.Generic;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Interfaces
{
    public interface IFollowingRepository
    {
        // Who is following me
        Task<List<Following>> GetAllFollowersOf(string userId);
        // Who I follow
        Task<List<Following>> GetAllFolloweesFrom(string userId);
        Task<Following> Create(Following following);
    }
}