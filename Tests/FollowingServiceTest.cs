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
        private TweetService _tweetService;

        [OneTimeSetUp]
        public void Init()
        {
            List<Tweet> tweets = new List<Tweet> 
            {
                new Tweet {UserId = "1", Text = "First"},
                new Tweet {UserId = "1", Text = "Second"}
            };
            var tweetServiceMock = new Mock<TweetService>();
            tweetServiceMock.Setup(t => t.GetTweetsBy("1"))
            .Returns(Task.FromResult(tweets));

            List<Following> followers = new List<Following>();
            followers.Add(new Following {FollowerId = "10", FolloweeId = "1"});
            followers.Add(new Following {FollowerId = "11", FolloweeId = "1"});
            followers.Add(new Following {FollowerId = "12", FolloweeId = "1"});

            List<Following> followees = new List<Following>();
            followees.Add(new Following {FollowerId = "2", FolloweeId = "20"}); 
            followees.Add(new Following {FollowerId = "2", FolloweeId = "21"}); 
            
            var repoMock = new Mock<IFollowingRepository>();
            repoMock.Setup(r => r.GetAllFollowersOf(It.IsAny<string>()))
            .Returns(Task.FromResult(followers));

            repoMock.Setup(r => r.GetAllFolloweesFrom(It.IsAny<string>()))
            .Returns(Task.FromResult(followees));

            _repository = repoMock.Object;

            _service = new FollowingService(_repository, tweetServiceMock.Object);
        }

        [Test]
        public async Task IsGettingAllFollowings()
        {
            List<Following> followers = await _service.GetAllFollowersOf("1");
            Assert.AreEqual(3, followers.Count);

            List<Following> followees = await _service.GetAllFolloweesOf("2");
            Assert.AreEqual(2, followees.Count);
        }
    }
}