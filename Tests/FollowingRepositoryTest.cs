using NUnit.Framework;
using System;
using TweetishApp.Models;
using TweetishApp.Core.Interfaces;
using TweetishApp.Core.Entities;
using TweetishApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TweetishApp.Data
{
    [TestFixture]
    public class FollowingRepositoryTest
    {
        private AppDbContext _dbContext;
        private FollowingRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "test_db")
            .Options;

            _dbContext = new AppDbContext(options);
            _repository = new FollowingRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            List<FollowingModel> models = _dbContext.Following.ToList();

            foreach (FollowingModel m in models) {
                _dbContext.Remove<FollowingModel>(m);
                _dbContext.SaveChanges();
            }
        }

        public void populate()
        {
            _dbContext.Add<FollowingModel>(
                new FollowingModel {
                    FollowerId = "1", FolloweeId = "10",
                    Follower = new AppUser {Nickname = "Terry"},
                    Followee = new AppUser {Nickname = "Ryu"}
                }
            );
            _dbContext.Add<FollowingModel>(
                new FollowingModel {FollowerId = "1", FolloweeId = "11"}
            );
            _dbContext.Add<FollowingModel>(
                new FollowingModel {FollowerId = "1", FolloweeId = "12"}
            );
            _dbContext.Add<FollowingModel>(
                new FollowingModel {FollowerId = "12", FolloweeId = "1"}
            );
            _dbContext.Add<FollowingModel>(
                new FollowingModel {FollowerId = "20", FolloweeId = "1"}
            );

            _dbContext.SaveChanges();
        }

        [Test]
        public async Task IsGettingAllFollowees()
        {
            this.populate();

            List<Following> followees = await _repository.GetAllFolloweesFrom("1");
            Assert.AreEqual(2, followees.Count);
        }

        [Test]
        public async Task IsGettingAllFollowers()
        {
            this.populate();

            List<Following> followers = await _repository.GetAllFollowersOf("1");
            Assert.AreEqual(2, followers.Count);
        }

        [Test]
        public async Task IsFollowingUser()
        {
            List<FollowingModel> models = _dbContext.Following.ToList();
            Assert.AreEqual(0, models.Count);

            Following following = new Following {FollowerId = "22", FolloweeId = "11"};
            following = await _repository.Create(following);
            models = _dbContext.Following.ToList();
            Assert.AreEqual(1, models.Count);
            Assert.AreEqual(1, following.Id);
            Assert.AreEqual("22", following.FollowerId);
            Assert.AreEqual("11", following.FolloweeId);
        }

        [Test]
        public async Task IsUnfollowingUser()
        {
            Following following = new Following {FollowerId = "2", FolloweeId = "1"};

            await _repository.Create(following);
            following.FolloweeId = "0";
            await _repository.Create(following);
            following.FolloweeId = "-1";
            await _repository.Create(following);

            List<FollowingModel> models = _dbContext.Following.ToList();
            Assert.AreEqual(3, models.Count);

            following.FollowerId = "2";
            following.FolloweeId = "0";

            await _repository.Remove(following);
            models = _dbContext.Following.ToList();
            Assert.AreEqual(2, models.Count);
        }

        [Test]
        public void IsThrowingWhenUnfollowingNotExist()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => {
                await _repository.Remove(new Following {FollowerId = "1", FolloweeId = "2"});
            });
        }
    }
}