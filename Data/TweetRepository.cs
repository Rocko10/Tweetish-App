using TweetishApp.Core.Interfaces;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using TweetishApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using TweetishApp.Models;
using System.Linq;

namespace TweetishApp.Data
{
    public class TweetRepository : ITweetRepository
    {
        private readonly AppDbContext _dbContext;

        public TweetRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tweet> Create(Tweet tweet)
        {
            TweetModel model = new TweetModel
            {
                UserId = tweet.UserId,
                Text = tweet.Text,
                CreatedAt = tweet.CreatedAt,
                UpdatedAt = tweet.UpdatedAt
            };

            _dbContext.Add<TweetModel>(model);
            await _dbContext.SaveChangesAsync();

            tweet.Id = model.Id;

            return tweet;
        }

        public async Task<Tweet> Update(Tweet tweet)
        {
            TweetModel model = await _dbContext.Tweet.FirstOrDefaultAsync(t => t.Id == tweet.Id);

            if (model == null) {
                throw new ArgumentNullException("model not found");
            }
            model.Text = tweet.Text;

            _dbContext.Update<TweetModel>(model);
            await _dbContext.SaveChangesAsync();
            tweet.Id = model.Id;

            return tweet;
        }

        public async Task Remove(int id)
        {
            TweetModel model = await _dbContext.Tweet.FirstOrDefaultAsync(t => t.Id == id);

            if (model == null) {
                throw new ArgumentNullException("Cannot found model");
            }

            _dbContext.Remove(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Tweet>> GetAllByUserId(string userId)
        {
            List<TweetModel> models = await _dbContext.Tweet
            .Where(t => t.UserId == userId)
            .ToListAsync();

            List<Tweet> tweets = new List<Tweet>();

            foreach (TweetModel m in models) {
                Tweet t = new Tweet(m.UserId);
                t.Id = m.Id;
                t.Text = m.Text;

                tweets.Add(t);
            }

            return tweets;
        }

        public async Task<List<Tweet>> GetTweetsBy(string userId)
        {
            AppUser user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) {
                throw new ArgumentNullException("User not found");
            }

            List<TweetModel> models = await _dbContext.Tweet
            .Where(t => t.UserId == userId)
            .ToListAsync();

            List<Tweet> tweets = new List<Tweet>();

            foreach (TweetModel m in models) {
                Tweet t = new Tweet
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Nickname = user.Nickname,
                    Text = m.Text,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt
                };

                tweets.Add(t);
            }

            return tweets;
        }
    }
}