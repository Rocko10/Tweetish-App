using NUnit.Framework;
using System.Threading.Tasks;
using TweetishApp.Data;
using System.Collections.Generic;
using Moq;
using TweetishApp.Core.Interfaces;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Services
{
    [TestFixture]
    public class FollowingServiceTest
    {
        private IFollowingRepository _repository;
        private FollowingService _service;

        [OneTimeSetUp]
        public void Init()
        {
            List<Tweet> tweets = new List<Tweet> 
            {
                new Tweet {UserId = "2", Text = "First"},
                new Tweet {UserId = "2", Text = "Second"}
            };
            var tweets2 = new List<Tweet>
            {
                new Tweet {UserId = "3", Text = "Third"}
            };
            var tweetServiceMock = new Mock<ITweetService>();
            tweetServiceMock.Setup(t => t.GetTweetsBy("2"))
            .Returns(Task.FromResult(tweets));
            tweetServiceMock.Setup(t => t.GetTweetsBy("3"))
            .Returns(Task.FromResult(tweets2));

            List<Following> followers = new List<Following>();
            followers.Add(new Following {FollowerId = "10", FolloweeId = "1"});
            followers.Add(new Following {FollowerId = "11", FolloweeId = "1"});
            followers.Add(new Following {FollowerId = "12", FolloweeId = "1"});

            List<Following> followees = new List<Following>();
            followees.Add(new Following {FollowerId = "20", FolloweeId = "2"}); 
            followees.Add(new Following {FollowerId = "20", FolloweeId = "3"}); 
            
            var repoMock = new Mock<IFollowingRepository>();
            repoMock.Setup(r => r.GetAllFollowersOf("1"))
            .Returns(Task.FromResult(followers));

            repoMock.Setup(r => r.GetAllFolloweesFrom("20"))
            .Returns(Task.FromResult(followees));

            // repoMock.Setup(r => r.ExistFollowing(new Following {FollowerId = "11", FolloweeId = "1"}))
            repoMock.Setup(r => r.ExistFollowing(It.IsAny<Following>()))
            .Returns(Task.FromResult(true));

            _repository = repoMock.Object;

            _service = new FollowingService(_repository, tweetServiceMock.Object);
        }

        [Test]
        public async Task IsGettingAllFollowings()
        {
            List<Following> followers = await _service.GetAllFollowersOf("1");
            Assert.AreEqual(3, followers.Count);

            List<Following> followees = await _service.GetAllFolloweesFrom("20");
            Assert.AreEqual(2, followees.Count);
        }

        [Test]
        public async Task IsGettingTweetsOfFollowees()
        {
            List<Tweet> tweets = await _service.GetFolloweesTweets("20");
            Assert.AreEqual(3, tweets.Count);
            Assert.AreEqual("First", tweets[0].Text);
            Assert.AreEqual("2", tweets[0].UserId);
            Assert.AreEqual("Second", tweets[1].Text);
            Assert.AreEqual("2", tweets[1].UserId);
            Assert.AreEqual("Third", tweets[2].Text);
            Assert.AreEqual("3", tweets[2].UserId);
        }
        
        [Test]
        public async Task IsCheckingIfExistFollowing()
        {
            Following following = new Following {FollowerId = "11", FolloweeId = "1"};

            bool exist = await _service.ExistFollowing(following);
            Assert.IsTrue(exist);
        }
    }
}