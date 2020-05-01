using TweetishApp.Data;
using System;
using TweetishApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TweetishApp.Services
{
    public class TweetService
    {
        private readonly AppDbContext _dbContext;

        public TweetService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tweet> Create(Tweet tweet)
        {
            if (tweet.UserId == null) {
                throw new ArgumentNullException("UserId cannot be null");
            }

            tweet.CreatedAt = DateTime.UtcNow;
            tweet.UpdatedAt = DateTime.UtcNow;

           _dbContext.Add<Tweet>(tweet);
           await _dbContext.SaveChangesAsync(); 

           return tweet;
        }

        public async Task<Tweet> Update(Tweet tweet)
        {
            Tweet model = await _dbContext.Tweet.FirstOrDefaultAsync(t => t.Id == tweet.Id);

            if (model == null) {
                throw new ArgumentNullException("No model found");
            }

            model.Text = tweet.Text;
            _dbContext.Update<Tweet>(model);
            await _dbContext.SaveChangesAsync();

            return model;
        }

        public async Task Remove(int id)
        {
            Tweet tweet = await _dbContext.Tweet.FirstOrDefaultAsync(t => t.Id == id);

            if (tweet == null) {
                throw new ArgumentNullException("No tweet found to delete");
            }

            _dbContext.Tweet.Remove(tweet);
            await _dbContext.SaveChangesAsync();
        }
    }
}