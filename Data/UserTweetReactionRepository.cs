using TweetishApp.Core.Interfaces;
using System;
using TweetishApp.Models;
using TweetishApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using TweetishApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TweetishApp.Data
{
    public class UserTweetReactionRepository : IUserTweetReactionRepository
    {
        private AppDbContext _dbContext;

        public UserTweetReactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserTweetReaction> Create(UserTweetReaction userTweetReaction)
        {
            AppUser user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userTweetReaction.UserId);
            TweetModel tweetModel = await _dbContext.Tweet.FirstOrDefaultAsync(t => t.Id == userTweetReaction.TweetId);
            ReactionModel reactionModel = await _dbContext.Reaction.FirstOrDefaultAsync(r => r.Id == userTweetReaction.ReactionId);

            if (user == null || tweetModel == null || reactionModel == null) {
                throw new ArgumentNullException("Invalid input");
            }

            UserTweetReactionModel model = new UserTweetReactionModel
            {
                UserId = userTweetReaction.UserId,
                TweetId = userTweetReaction.TweetId,
                ReactionId = userTweetReaction.ReactionId
            };

            _dbContext.Add<UserTweetReactionModel>(model);
            await _dbContext.SaveChangesAsync();

            userTweetReaction.Id = model.Id;
            userTweetReaction.User = user;
            userTweetReaction.Tweet = new Tweet { Id = tweetModel.Id, Text = tweetModel.Text };
            userTweetReaction.Reaction = new Reaction(name: reactionModel.Name);
            userTweetReaction.Reaction.Id = reactionModel.Id;

            return userTweetReaction;
        }
    }
}