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
                    Follower = new FollowerModel { Id = "1", Nickname = "joe" }, Followee = new FolloweeModel { Id = "10", Nickname = "ben" }
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
            Assert.AreEqual(3, followees.Count);
        }

        [Test]
        public async Task IsGettingAllFollowers()
        {
            this.populate();

            List<Following> followers = await _repository.GetAllFollowersOf("1");
            Assert.AreEqual(2, followers.Count);
        }
    }
}