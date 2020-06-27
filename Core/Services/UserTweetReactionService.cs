using TweetishApp.Core.Interfaces;
using System.Collections.Generic;
using TweetishApp.Core.Entities;
using System.Threading.Tasks;

namespace TweetishApp.Core.Services
{
    public class UserTweetReactionService : IUserTweetReactionService
    {
        private IUserTweetReactionRepository _repository;

        public UserTweetReactionService(IUserTweetReactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserTweetReaction> Toggle(UserTweetReaction userTweetReaction)
        {
            return await _repository.Toggle(userTweetReaction);
        }

        public async Task<UserTweetReaction> Reacted(UserTweetReaction userTweetReaction)
        {
            return await _repository.Reacted(userTweetReaction);
        }

        public async Task<IEnumerable<UserTweetReaction>> ReactedToMany(IEnumerable<UserTweetReaction> reactions)
        {
            return await _repository.ReactedToMany(reactions);
        }
    }
}