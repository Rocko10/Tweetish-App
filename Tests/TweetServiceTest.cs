using NUnit.Framework;
using System;
using TweetishApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TweetishApp.Data;

namespace TweetishApp.Services
{
    [TestFixture]
    public class TweetServiceTest
    {
        private TweetService _service;
        private AppDbContext _dbContext;

        [OneTimeSetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "in_memory_db_test")
            .Options;

            _dbContext = new AppDbContext(options);
            _service = new TweetService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            List<Tweet> tweets = _dbContext.Tweet.ToList();
            foreach (Tweet t in tweets) {
                _dbContext.Remove(t);
            }
            _dbContext.SaveChanges();
        }


        [Test]
        public async Task IsCreatingTweet()
        {
            Tweet tweet = new Tweet {Text = "First tweet", UserId = "123"};
            Assert.AreNotEqual("0001-01-01 00:00:00", tweet.CreatedAt);
            Assert.AreEqual(0, tweet.Id);

            tweet = await _service.Create(tweet);
            Assert.AreNotEqual(0, tweet.Id);
            Assert.AreNotEqual("0001-01-01 00:00:00", tweet.CreatedAt);
        }

        [Test]
        public void IsThrowingWhenNoUserIdWhileCreatingTweet()
        {
            Tweet tweet = new Tweet {Text = "Will fail"};

            Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await _service.Create(tweet);
            });
        }
    }
}