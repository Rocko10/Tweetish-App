using NUnit.Framework;
using TweetishApp.Core.Entities;
using System;
using TweetishApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TweetishApp.Data;
using TweetishApp.Core.Interfaces;
using Moq;

namespace TweetishApp.Core.Services
{
    [TestFixture]
    public class TweetServiceTest
    {
        private TweetService _service;

        [OneTimeSetUp]
        public void Init()
        {
            var repo = new Mock<ITweetRepository>();
            Tweet tweet = new Tweet(userId: "123");
            tweet.Id = 1;
            tweet.Text = "Test tweet";
            tweet.CreatedAt = DateTime.UtcNow;
            tweet.UpdatedAt = DateTime.UtcNow;

            List<Tweet> tweets = new List<Tweet>();
            tweets.Add(tweet);

            repo.Setup(r => r.Create(It.IsAny<Tweet>()))
            .Returns(Task.FromResult(tweet));

            repo.Setup(r => r.GetTweetsBy("123"))
            .Returns(Task.FromResult(tweets));

            _service = new TweetService(repo.Object);
        }


        [Test]
        public async Task IsCreatingTweetInService()
        {
            Tweet tweet = new Tweet(userId: "123");
            tweet.Text = "super tweet";
            Assert.AreNotEqual("0001-01-01 00:00:00", tweet.CreatedAt);
            Assert.AreEqual(0, tweet.Id);

            tweet = await _service.Create(tweet);
            Assert.AreNotEqual(0, tweet.Id);
            Assert.AreNotEqual("0001-01-01 00:00:00", tweet.CreatedAt);
        }

        [Test]
        public void IsThrowingOnInvalidTweet()
        {
            Tweet tweet = new Tweet();
            Assert.ThrowsAsync<ArgumentException>(async () => {
                await _service.Create(tweet);
            });

            tweet.UserId = "123";
            tweet.Text = "a";

            Assert.ThrowsAsync<ArgumentException>(async () => {
                await _service.Create(tweet);
            });
        }

        [Test]
        public async Task IsReturningTweetByUserId()
        {
            List<Tweet> tweets = await _service.GetTweetsBy("123");
            Assert.AreEqual(1, tweets.Count);
            Assert.AreEqual("Test tweet", tweets[0].Text);
            Assert.AreEqual("123", tweets[0].UserId);
        }
    }
}