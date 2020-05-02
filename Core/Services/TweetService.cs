using System.Threading.Tasks;
using System.Collections.Generic;
using TweetishApp.Core.Interfaces;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Services
{
    public class TweetService
    {
        private readonly ITweetRepository _repository;

        public TweetService(ITweetRepository repository)
        {
            _repository = repository;
        }

        public async Task<Tweet> Create(Tweet tweet)
        {
            tweet = await _repository.Create(tweet);

           return tweet;
        }

        public async Task<Tweet> Update(Tweet tweet)
        {
            tweet = await _repository.Update(tweet);

            return tweet;
        }

        public async Task Remove(int id)
        {
            await _repository.Remove(id);
        }
    }
}