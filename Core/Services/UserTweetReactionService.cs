using TweetishApp.Core.Interfaces;
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
    }
}