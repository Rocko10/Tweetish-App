using System.Threading.Tasks;
using System.Collections.Generic;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Interfaces
{
    public interface ITweetService
    {
        Task<Tweet> Create(Tweet tweet);
        Task<Tweet> Update(Tweet tweet);
        Task Remove(int id);
        Task<List<Tweet>> GetTweetsBy(string userId);
    }
}