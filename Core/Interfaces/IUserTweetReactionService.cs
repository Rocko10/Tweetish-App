using System.Threading.Tasks;
using System.Collections.Generic;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Interfaces
{
    public interface IUserTweetReactionService
    {
        Task<UserTweetReaction> Toggle(UserTweetReaction userTweetReaction);
        Task<UserTweetReaction> Reacted(UserTweetReaction userTweetReaction);
        Task<IEnumerable<UserTweetReaction>> ReactedToMany(IEnumerable<UserTweetReaction> reactions);
    }
}