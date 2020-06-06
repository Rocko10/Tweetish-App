using TweetishApp.Core.Interfaces;
using System;
using TweetishApp.Models;
using TweetishApp.Core.Entities;
using TweetishApp.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace TweetishApp.Data
{
    public class RetweetRepository : IRetweetRepository
    {
        private AppDbContext _dbContext;

        public RetweetRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Retweet> Create(Retweet retweet)
        {
            RetweetModel model = await _dbContext.Retweet
            .FirstOrDefaultAsync(r => r.TweetId == retweet.TweetId && r.UserId == retweet.UserId);

            if (model != null) {
                throw new ArgumentException("Retweet already exist");
            }

            model = new RetweetModel
            {
                UserId = retweet.UserId,
                TweetId = retweet.TweetId
            };

            _dbContext.Add<RetweetModel>(model);
            await _dbContext.SaveChangesAsync();

            return new Retweet {
                Id = model.Id,
                UserId = model.UserId,
                TweetId = model.TweetId
            };
        }

        public async Task Remove(Retweet retweet)
        {
            RetweetModel model = await _dbContext.Retweet
            .FirstOrDefaultAsync(r => r.UserId == retweet.UserId && r.TweetId == retweet.TweetId);

            if (model == null) {
                throw new ArgumentException("Retweet not foud");
            }

            _dbContext.Remove<RetweetModel>(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Toggle(Retweet retweet)
        {
            AppUser user = _dbContext.Users.FirstOrDefault(u => u.Id == retweet.UserId);

            if (user == null) {
                throw new ArgumentNullException("User not found");
            }

            TweetModel tweetModel = _dbContext.Tweet
            .FirstOrDefault(t => t.Id == retweet.TweetId && t.UserId != retweet.UserId);

            if (tweetModel == null) {
                throw new ArgumentNullException("Invalid tweet");
            }

            RetweetModel model = await _dbContext.Retweet
            .FirstOrDefaultAsync(r => r.UserId == retweet.UserId && r.TweetId == retweet.TweetId);

            if (model != null) {
                _dbContext.Remove<RetweetModel>(model);
                await _dbContext.SaveChangesAsync();

                return;
            }

            model = new RetweetModel {
                TweetId = retweet.TweetId,
                UserId = retweet.UserId
            };

            _dbContext.Add<RetweetModel>(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Retweet> GetInfo(Retweet retweet)
        {
            RetweetModel model = await _dbContext.Retweet
            .Include(r => r.User)
            .Include(r => r.Tweet)
            .FirstOrDefaultAsync(r => r.UserId == retweet.UserId && r.TweetId == retweet.TweetId);

            if (model == null) {
                throw new ArgumentNullException("Retweet not found while retrieving info."); 
            }

            return new Retweet {
                Id = model.Id,
                TweetId = model.TweetId,
                UserId = model.UserId,
                User = model.User,
                Tweet = new Tweet { Id = model.Tweet.Id, Text = model.Tweet.Text, UserId = model.UserId }
            };
        }

        public async Task<List<Tweet>> GetRetweetsByUserId(string userId)
        {
            List<RetweetModel> retweetModels = await _dbContext.Retweet
            .Include(r => r.Tweet)
            .ThenInclude(t => t.User)
            .Where(r => r.UserId == userId)
            .ToListAsync();

            List<Tweet> tweets = new List<Tweet>();

            foreach (RetweetModel r in retweetModels) {
                Tweet t = new Tweet 
                {Id = r.Tweet.Id, UserId = r.Tweet.UserId, Text = r.Tweet.Text, Nickname = r.Tweet.User.Nickname};
                tweets.Add(t);
            }

            return tweets;
        }
    }
}