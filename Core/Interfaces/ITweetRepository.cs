using TweetishApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TweetishApp.Core.Interfaces
{
    public interface ITweetRepository
    {
        Task<Tweet> Create(Tweet tweet); 
        Task<Tweet> Update(Tweet tweet);
        Task Remove(int id);
    }
}