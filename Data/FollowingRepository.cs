using TweetishApp.Core.Interfaces;
using TweetishApp.Models;
using Microsoft.EntityFrameworkCore;
using TweetishApp.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace TweetishApp.Data
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly AppDbContext _dbContext;

        public FollowingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Following>> GetAllFollowersOf(string userId)
        {
            List<FollowingModel> models = await _dbContext.Following
            .Where(f => f.FolloweeId == userId)
            .ToListAsync();
            List<Following> followers = new List<Following>();

            foreach (FollowingModel m in models) {
                Following f = new Following {
                    FolloweeId = m.FolloweeId,
                    FollowerId = m.FollowerId
                };
                followers.Add(f);
            }

            return followers;
        }

        // People who userId follows
        public async Task<List<Following>> GetAllFolloweesFrom(string userId)
        {
            List<Following> followees = new List<Following>();
            List<FollowingModel> models = await _dbContext.Following
            .Where(f => f.FollowerId == userId)
            .ToListAsync();

            foreach (FollowingModel m in models) {
                Following f = new Following {
                    FollowerId = m.FollowerId,
                    FolloweeId = m.FolloweeId
                };

                followees.Add(f);
            }

            return followees;
        }

        public async Task<Following> Create(Following following)
        {
            FollowingModel model = new FollowingModel {
                FollowerId = following.FollowerId,
                FolloweeId = following.FolloweeId
            };

            _dbContext.Following.Add(model);
            await _dbContext.SaveChangesAsync();

            return new Following {
                Id = model.Id,
                FollowerId = model.FollowerId,
                FolloweeId = model.FolloweeId
            };
        }

        public async Task Remove(Following following)
        {
            FollowingModel model = await _dbContext.Following
            .FirstOrDefaultAsync(f => f.FollowerId == following.FollowerId && f.FolloweeId == following.FolloweeId);

            if (model == null) {
                throw new ArgumentException("Following not exist");
            }

            _dbContext.Following.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }

}