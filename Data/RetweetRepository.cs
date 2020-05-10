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
    }
}