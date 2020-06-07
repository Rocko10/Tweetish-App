using System.Threading.Tasks;
using System.Collections.Generic;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;

namespace TweetishApp.Core.Services
{
    public class RetweetService : IRetweetService
    {
        private readonly IRetweetRepository _repository;

        public RetweetService(IRetweetRepository repository)
        {
            _repository = repository;
        }

        public async Task<Retweet> Create(Retweet retweet)
        {
            return await _repository.Create(retweet);
        }

        public async Task Remove(Retweet retweet)
        {
            await _repository.Remove(retweet);
        }

        public async Task Toggle(Retweet retweet)
        {
            await _repository.Toggle(retweet);
        }

        public async Task<Retweet> GetInfo(Retweet retweet)
        {
            return await _repository.GetInfo(retweet);
        }

        public async Task<List<Tweet>> GetRetweetsByUserId(string userId)
        {
            return await _repository.GetRetweetsByUserId(userId);
        }

        public async Task<bool> IsRetweeted(string userId, int tweetId)
        {
            return await _repository.IsRetweeted(userId, tweetId);
        }
    }
}