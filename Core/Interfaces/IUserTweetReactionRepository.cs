using TweetishApp.Core.Entities;
using System.Threading.Tasks;

namespace TweetishApp.Core.Interfaces
{
    public interface IUserTweetReactionRepository
    {
        Task<UserTweetReaction> Create(UserTweetReaction userTweetReaction);
    }
}