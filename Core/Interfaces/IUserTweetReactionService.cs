using System.Threading.Tasks;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Interfaces
{
    public interface IUserTweetReactionService
    {
        Task<UserTweetReaction> Toggle(UserTweetReaction userTweetReaction);
        Task<UserTweetReaction> Reacted(UserTweetReaction userTweetReaction);
    }
}