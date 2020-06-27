using NUnit.Framework;
using System;
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

            List<AppUser> users = _dbContext.Users.ToList();
            List<ReactionModel> reactions = _dbContext.Reaction.ToList();
            List<TweetModel> tweets = _dbContext.Tweet.ToList();

            foreach (AppUser u in users) {
                _dbContext.Remove<AppUser>(u);
            }

            foreach (ReactionModel r in reactions) {
                _dbContext.Remove<ReactionModel>(r);
            }

            foreach (TweetModel t in tweets) {
                _dbContext.Remove<TweetModel>(t);
            }

            _dbContext.SaveChanges();
        }

        private (AppUser, TweetModel, ReactionModel) populate()
        {
            AppUser user = new AppUser {Nickname = "marcow"};
            ReactionModel reaction = new ReactionModel { Name = "Star" };
            _dbContext.Add<AppUser>(user);
            _dbContext.Add<ReactionModel>(reaction);
            _dbContext.SaveChanges();
            TweetModel tweet = new TweetModel {UserId = user.Id, Text = "Super First Tweet"};
            _dbContext.Add<TweetModel>(tweet);
            _dbContext.SaveChanges();

            return (user, tweet, reaction);
        }

        private void populateMany()
        {
            AppUser user = new AppUser {Nickname = "marcow"};
            AppUser joe = new AppUser {Nickname = "joe"};
            _dbContext.Add<AppUser>(user);
            _dbContext.Add<AppUser>(joe);

            ReactionModel star = new ReactionModel { Name = "Star" };
            ReactionModel heart = new ReactionModel { Name = "Heart" };
            ReactionModel cross = new ReactionModel { Name = "Cross" };
            _dbContext.Add<ReactionModel>(star);
            _dbContext.Add<ReactionModel>(heart);
            _dbContext.Add<ReactionModel>(cross);

            _dbContext.SaveChanges();

            TweetModel tweet1 = new TweetModel {UserId = user.Id, Text = "Super First Tweet"};
            TweetModel tweet2 = new TweetModel {UserId = joe.Id, Text = "Joe s tweet"};
            _dbContext.Add<TweetModel>(tweet1);
            _dbContext.Add<TweetModel>(tweet2);
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

            reaction = await _repository.Toggle(reaction);
            models = await _dbContext.UserTweetReaction.ToListAsync();
            Assert.AreEqual("marcow", reaction.User.Nickname);
            Assert.AreEqual("Super First Tweet", reaction.Tweet.Text);
            Assert.AreEqual("Star", reaction.Reaction.Name);
            Assert.AreNotEqual(0, reaction.Id);
            Assert.AreEqual(1, models.Count);

            reaction = await _repository.Toggle(reaction);
            Assert.IsNull(reaction);
            models = await _dbContext.UserTweetReaction.ToListAsync();
            Assert.AreEqual(0, models.Count);
        }

        [Test]
        public async Task IsRemovingUserTweetReactionInToggle()
        {
            this.populate();

            List<UserTweetReactionModel> models = _dbContext.UserTweetReaction.ToList();
            Assert.AreEqual(0, models.Count);

            AppUser user = _dbContext.Users.FirstOrDefault(u => u.Nickname == "marcow");
            TweetModel tweet = _dbContext.Tweet.FirstOrDefault(t => t.UserId == user.Id);
            ReactionModel reaction = _dbContext.Reaction.FirstOrDefault(r => r.Name == "Star");

            UserTweetReaction userTweetReaction = new UserTweetReaction 
            { UserId = user.Id, TweetId = tweet.Id, ReactionId = reaction.Id };

            await _repository.Toggle(userTweetReaction);
            models = _dbContext.UserTweetReaction.ToList();
            Assert.AreEqual(1, models.Count);

            userTweetReaction.Id = 0;
            await _repository.Toggle(userTweetReaction);
            models = _dbContext.UserTweetReaction.ToList();
            Assert.AreEqual(0, models.Count);
        }

        [Test]
        public async Task IsUserBeingReactingToTweet()
        {
            (AppUser, TweetModel, ReactionModel) data = this.populate();
            UserTweetReaction reaction = new UserTweetReaction
            {
                UserId = data.Item1.Id,
                TweetId = data.Item2.Id,
                ReactionId = data.Item3.Id,
            };

            List<UserTweetReactionModel> models = _dbContext.UserTweetReaction.ToList();
            Assert.AreEqual(0, models.Count);

            await _repository.Toggle(reaction);
            models = _dbContext.UserTweetReaction.ToList();
            Assert.AreEqual(1, models.Count);

            UserTweetReaction reacted = await _repository.Reacted(
                new UserTweetReaction {UserId = data.Item1.Id, TweetId = data.Item2.Id, ReactionId = data.Item3.Id}
            );
            Assert.True(reacted.Reacted);
        }

        [Test]
        public async Task IsReactingToManyInRepo()
        {
            this.populateMany();

            AppUser marcow = _dbContext.Users.FirstOrDefault(u => u.Nickname == "marcow");
            TweetModel tweet1 = _dbContext.Tweet.FirstOrDefault(t => t.UserId == marcow.Id);
            ReactionModel star = _dbContext.Reaction.FirstOrDefault(r => r.Name == "Star");
            ReactionModel heart = _dbContext.Reaction.FirstOrDefault(r => r.Name == "Heart");
            ReactionModel cross = _dbContext.Reaction.FirstOrDefault(r => r.Name == "Cross");

            var r1 = new UserTweetReaction {UserId = marcow.Id, TweetId = tweet1.Id, ReactionId = star.Id};
            var r2 = new UserTweetReaction {UserId = marcow.Id, TweetId = tweet1.Id, ReactionId = heart.Id};
            var r3 = new UserTweetReaction {UserId = marcow.Id, TweetId = tweet1.Id, ReactionId = cross.Id};

            await _repository.Toggle(r1);
            await _repository.Toggle(r2);
            await _repository.Toggle(r3);

            IEnumerable<UserTweetReactionModel> setupReactions = _dbContext.UserTweetReaction.ToList();
            Assert.AreEqual(3, setupReactions.Count());

            var requestReactions = new UserTweetReaction[] {r1, r1, r3};

            IEnumerable<UserTweetReaction> resultReacions = await _repository.ReactedToMany(requestReactions);

            foreach (UserTweetReaction u in resultReacions) {
                Assert.True(u.Reacted);
            }
        }
    }
}