using NUnit.Framework;
using System;
using System.Collections.Generic;
using TweetishApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TweetishApp.Core.Entities;
using System.Linq;

namespace TweetishApp.Data
{
    [TestFixture]
    public class TweetRepositoryTest
    {
        private AppDbContext _dbContext;
        private TweetRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "in_memory_db")
            .Options;

            _dbContext = new AppDbContext(options);
            _repository = new TweetRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            List<TweetModel> tweets = _dbContext.Tweet.ToList();
            foreach (TweetModel t in tweets) {
                _dbContext.Remove(t);
            }
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task IsCreatingTweetInRepo()
        {
            Tweet tweet = new Tweet(userId: "123");
            Assert.AreEqual(0, tweet.Id);

            tweet = await _repository.Create(tweet);
            Assert.AreNotEqual(0, tweet.Id);
        }

        [Test]
        public async Task IsUpdatingTweetInRepo()
        {
            Tweet tweet = new Tweet(userId: "123");
            tweet.Text = "Original";

            tweet = await _repository.Create(tweet);
            tweet.Text = "Updated";

            await _repository.Update(tweet);

            TweetModel model = await _dbContext.Tweet.FirstOrDefaultAsync(t => t.Id == tweet.Id);
            Assert.AreEqual("Updated", model.Text);
        }

        [Test]
        public void IsThrowingOnUpdateIfNotFound()
        {
            Tweet tweet = new Tweet(userId: "124");
            Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await _repository.Update(tweet);
            });
        }

        [Test]
        public async Task IsRemovingTweetInRepo()
        {
            Tweet tweet = new Tweet(userId: "123");
            tweet.Text = "text";
            tweet = await _repository.Create(tweet); 

            List<TweetModel> tweets = _dbContext.Tweet.ToList();
            Assert.AreEqual(1, tweets.Count);

            await _repository.Remove(tweet.Id);

            tweets = _dbContext.Tweet.ToList();
            Assert.AreEqual(0, tweets.Count);
        }

        [Test]
        public void IsThrowingWhenIdNotExistOnRemoving()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await _repository.Remove(10);
            });
        }

        [Test]
        public async Task IsGettingTweetsByUser()
        {
            Tweet tweet = new Tweet {UserId = "123", Text = "First"};
            await _repository.Create(tweet);
            tweet.Text = "Second";
            await _repository.Create(tweet);
            tweet.Text = "Third";
            await _repository.Create(tweet);

            List<Tweet> tweets = await _repository.GetAllByUserId("123");
            Assert.AreEqual(3, tweets.Count);
        }
    }
}