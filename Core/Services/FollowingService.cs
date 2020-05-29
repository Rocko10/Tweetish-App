using System.Collections.Generic;
using System;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;
using System.Threading.Tasks;

namespace TweetishApp.Core.Services
{
    public class FollowingService : IFollowingService
    {
        private readonly IFollowingRepository _repository;
        private ITweetService _tweetService; 

        public FollowingService(
            IFollowingRepository repository,
            ITweetService tweetService
        )
        {
            _repository = repository;
            _tweetService = tweetService;
        }

        public async Task<List<Following>> GetAllFollowersOf(string userId)
        {
            return await _repository.GetAllFollowersOf(userId);
        }

        public async Task<List<Following>> GetAllFolloweesFrom(string userId)
        {
            return await _repository.GetAllFolloweesFrom(userId);
        }

        // Given an userId, returns tweets of the people who follow that userId
        public async Task<List<Tweet>> GetFolloweesTweets(string userId)
        {
            List<Following> followees = await this.GetAllFolloweesFrom(userId);
            List<Tweet> tweets = new List<Tweet>();

            foreach (Following f in followees) {
                List<Tweet> currentTweets = await _tweetService.GetTweetsBy(f.FolloweeId);
                tweets.AddRange(currentTweets);
            }

            return tweets;
        }

        public async Task<Following> Create(Following following)
        {
            return await _repository.Create(following);
        }

        public async Task Remove(Following following)
        {
            await _repository.Remove(following);
        }

        public async Task<bool> ExistFollowing(Following following)
        {
            return await _repository.ExistFollowing(following);
        }
    }
}