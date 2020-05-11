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
                _dbContext.SaveChanges();
            }
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
            Assert.AreEqual(1, retweet.Id);
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
            Retweet retweet = new Retweet {
                UserId = "123",
                TweetId = 1
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
    }
}