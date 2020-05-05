using System.Collections.Generic;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;
using System.Threading.Tasks;

namespace TweetishApp.Core.Services
{
    public class FollowingService
    {
        private readonly IFollowingRepository _repository;
        private TweetService _tweetService; 

        public FollowingService(
            IFollowingRepository repository,
            TweetService tweetService
        )
        {
            _repository = repository;
            _tweetService = tweetService;
        }

        public async Task<List<Following>> GetAllFollowersOf(string userId)
        {
            return await _repository.GetAllFollowersOf(userId);
        }

        public async Task<List<Following>> GetAllFolloweesOf(string userId)
        {
            return await _repository.GetAllFolloweesFrom(userId);
        }
    }
}