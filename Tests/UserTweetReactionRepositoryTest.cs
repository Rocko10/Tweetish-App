using NUnit.Framework;
using TweetishApp.Core.Entities;
using System.Collections.Generic;
using TweetishApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TweetishApp.Data;

namespace TweetishApp.Data
{
    [TestFixture]
    public class UserTweetReactionRepositoryTest
    {
        private AppDbContext _dbContext;
        private UserTweetReactionRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: "user_tweet_reaction_test")
            .Options;

            _dbContext = new AppDbContext(options);
            _repository = new UserTweetReactionRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            List<UserTweetReactionModel> models = _dbContext.UserTweetReaction.ToList();

            foreach (UserTweetReactionModel m in models) {
                _dbContext.Remove<UserTweetReactionModel>(m);
            }

            _dbContext.SaveChanges();
        }

        private void populate()
        {
            AppUser user = new AppUser {Nickname = "marcow"};
            ReactionModel reaction = new ReactionModel { Name = "Star" };
            _dbContext.Add<AppUser>(user);
            _dbContext.Add<ReactionModel>(reaction);
            _dbContext.SaveChanges();
            TweetModel tweet = new TweetModel {UserId = user.Id, Text = "Super First Tweet"};
            _dbContext.Add<TweetModel>(tweet);
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task IsCreatingUserTweetReactionInRepo()
        {
            this.populate();
            AppUser user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Nickname == "marcow");

            UserTweetReaction reaction = new UserTweetReaction
            {
                UserId = user.Id,
                TweetId = 1,
                ReactionId = 1
            };

            List<UserTweetReactionModel> models = await _dbContext.UserTweetReaction.ToListAsync();

            Assert.AreEqual(0, reaction.Id);
            Assert.AreEqual(0, models.Count);

            reaction = await _repository.Create(reaction);
            models = await _dbContext.UserTweetReaction.ToListAsync();
            Assert.AreEqual("marcow", reaction.User.Nickname);
            Assert.AreEqual("Super First Tweet", reaction.Tweet.Text);
            Assert.AreEqual("Star", reaction.Reaction.Name);
            Assert.AreNotEqual(0, reaction.Id);
            Assert.AreEqual(1, models.Count);
        }
    }
}