using TweetishApp.Data;
using System.Threading.Tasks;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;

namespace TweetishApp.Core.Services
{
    public class RetweetService : IRetweetService
    {
        private readonly RetweetRepository _repository;

        public RetweetService(RetweetRepository repository)
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
    }
}