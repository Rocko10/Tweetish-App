using TweetishApp.Core.Entities;
using System.Threading.Tasks;

namespace TweetishApp.Core.Interfaces
{
    public interface IUserTweetReactionRepository
    {
        Task<UserTweetReaction> Toggle(UserTweetReaction userTweetReaction);
        Task<bool> Reacted(UserTweetReaction userTweetReaction);
    }
}