using NUnit.Framework;
using System;
using TweetishApp.Core.Entities;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using TweetishApp.Models;
using TweetishApp.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TweetishApp.Data
{
    [TestFixture]
    public class RetweetRepositoryTest
    {
        private AppDbContext _dbContext;
        private RetweetRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "test_retweet")
            .Options;

            _dbContext = new AppDbContext(options);
            _repository = new RetweetRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            List<RetweetModel> models = _dbContext.Retweet.ToList();
            
            foreach (RetweetModel m in models) {
                _dbContext.Remove<RetweetModel>(m);
            }

            List<TweetModel> tweets = _dbContext.Tweet.ToList();

            foreach (TweetModel t in tweets) {
                _dbContext.Remove<TweetModel>(t);
            }

            List<AppUser> users = _dbContext.Users.ToList();

            foreach (AppUser u in users) {
                _dbContext.Remove<AppUser>(u);
            }

            _dbContext.SaveChanges();
        }

        private void populate()
        {
            AppUser u1 = new AppUser {Nickname = "marcow"};
            AppUser u2 = new AppUser {Nickname = "joe"};

            _dbContext.Add<AppUser>(u1);
            _dbContext.Add<AppUser>(u2);
            _dbContext.SaveChanges();

            TweetModel t1 = new TweetModel {UserId = u1.Id, Text = "I like turtles!"};
            _dbContext.Add<TweetModel>(t1);
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task IsRetweetingInRepository()
        {
            List<RetweetModel> models = await _dbContext.Retweet.ToListAsync();
            Assert.AreEqual(0, models.Count);

            Retweet retweet = new Retweet {
                UserId = "123",
                TweetId = 1
            };
            Assert.AreEqual(0, retweet.Id);
            retweet = await _repository.Create(retweet);
            Assert.AreNotEqual(0, retweet.Id);
            Assert.AreEqual("123", retweet.UserId);
            Assert.AreEqual(1, retweet.TweetId);
        }

        [Test]
        public async Task IsThrowingIfRetweetExistsInRepository()
        {
            Retweet retweet = new Retweet {
                UserId = "123",
                TweetId = 1
            };
            await _repository.Create(retweet);

            Assert.ThrowsAsync<ArgumentException>(async () => {
                await _repository.Create(retweet);
            });
        }

        [Test]
        public async Task IsUndoingRetweetInRepository()
        {
            Retweet retweet = new Retweet {
                UserId = "123",
                TweetId = 1
            };
            await _repository.Create(retweet);
            List<RetweetModel> models = _dbContext.Retweet.ToList();
            Assert.AreEqual(1, models.Count);

            await _repository.Remove(retweet);
            models = _dbContext.Retweet.ToList();
            Assert.AreEqual(0, models.Count);
        }

        [Test]
        public void IsThrowingInNonExistingRetweetUndoInRepo()
        {
            Retweet retweet = new Retweet {
                UserId = "123",
                TweetId = 1
            };

            Assert.ThrowsAsync<ArgumentException>(async () => {
                await _repository.Remove(retweet);
            });
        }

        [Test]
        public async Task IsTogglingRetweetInRepository()
        {
            AppUser user1 = new AppUser {Nickname = "Jhon"};
            AppUser user2 = new AppUser {Nickname = "Stu"};
            _dbContext.Add<AppUser>(user1);
            _dbContext.Add<AppUser>(user2);

            TweetModel tweetModel = new TweetModel {UserId = user1.Id, Text = "Super text"};

            _dbContext.Add<TweetModel>(tweetModel);
            _dbContext.SaveChanges();

            Retweet retweet = new Retweet {
                UserId = user2.Id,
                TweetId = tweetModel.Id
            };

            List<RetweetModel> models = _dbContext.Retweet.ToList();
            Assert.AreEqual(0, models.Count);

            await _repository.Toggle(retweet);
            models = _dbContext.Retweet.ToList();
            Assert.AreEqual(1, models.Count);

            await _repository.Toggle(retweet);
            models = _dbContext.Retweet.ToList();
            Assert.AreEqual(0, models.Count);
        }

        [Test]
        public async Task IsGettingRetweetInfoInRepository()
        {
            AppUser userModel = new AppUser { Nickname = "marcow" };
            _dbContext.Users.Add(userModel);
            Assert.NotNull(userModel.Id);

            TweetModel tweetModel = new TweetModel { UserId = userModel.Id, Text = "Super tweet!" };
            _dbContext.Add<TweetModel>(tweetModel);
            _dbContext.SaveChanges();

            Retweet retweet = new Retweet {UserId = userModel.Id, TweetId = tweetModel.Id};
            await _repository.Create(retweet);
            
            retweet = await _repository.GetInfo(retweet);
            Assert.AreEqual(userModel.Id, retweet.User.Id);
            Assert.AreEqual(tweetModel.Text, retweet.Tweet.Text);
        }

        [Test]
        public void IsThrowingOnRetweetOwnTweetInToggle()
        {
            AppUser userModel = new AppUser { Nickname = "marcow" };
            _dbContext.Users.Add(userModel);
            Assert.NotNull(userModel.Id);

            TweetModel tweetModel = new TweetModel { UserId = userModel.Id, Text = "Super tweet!" };
            _dbContext.Add<TweetModel>(tweetModel);
            _dbContext.SaveChanges();

            Assert.ThrowsAsync<ArgumentNullException>(async () => {
                Retweet retweet = new Retweet {UserId = userModel.Id, TweetId = tweetModel.Id};
                await _repository.Toggle(retweet);
            });
        }

        [Test]
        public async Task IsGettingRetweetsFromUserId()
        {
            this.populate();
            AppUser joe = _dbContext.Users.FirstOrDefault(u => u.Nickname == "joe");
            AppUser marcow = _dbContext.Users.FirstOrDefault(u => u.Nickname == "marcow");
            TweetModel t1 = _dbContext.Tweet.FirstOrDefault(t => t.Text == "I like turtles!");
            Retweet retweet = new Retweet {UserId = joe.Id, TweetId = t1.Id};

            List<RetweetModel> allRetweets = _dbContext.Retweet.ToList();
            Assert.AreEqual(0, allRetweets.Count);

            await _repository.Create(retweet);

            allRetweets = _dbContext.Retweet.ToList();
            Assert.AreEqual(1, allRetweets.Count);

            List<Tweet> joeRetweets = await _repository.GetRetweetsByUserId(joe.Id);
            Assert.AreEqual(1, joeRetweets.Count);
        }
    }
}