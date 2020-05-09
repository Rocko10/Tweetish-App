using System.Collections.Generic;
using System;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;
using System.Threading.Tasks;

namespace TweetishApp.Core.Services
{
    public class FollowingService
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

        public async Task<List<Following>> GetAllFolloweesOf(string userId)
        {
            return await _repository.GetAllFolloweesFrom(userId);
        }

        public async Task<List<Tweet>> GetFolloweesTweets(string userId)
        {
            List<Following> followees = await this.GetAllFolloweesOf(userId);
            List<Tweet> tweets = new List<Tweet>();

            foreach (Following f in followees) {
                List<Tweet> currentTweets = await _tweetService.GetTweetsBy(f.FolloweeId);
                tweets.AddRange(currentTweets);
            }

            return tweets;
        }
    }
}