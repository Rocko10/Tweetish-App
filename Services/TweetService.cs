using TweetishApp.Data;
using System;
using TweetishApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

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

    }
}