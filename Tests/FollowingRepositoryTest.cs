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
            List<AppUser> users = _dbContext.Users.ToList();

            foreach (FollowingModel m in models) {
                _dbContext.Remove<FollowingModel>(m);
            }

            foreach (AppUser u in users) {
                _dbContext.Remove<AppUser>(u);
            }

            _dbContext.SaveChanges();
        }

        public void populate()
        {
            AppUser u1 = new AppUser {Nickname = "Jim"};
            AppUser u2 = new AppUser {Nickname = "Bob"};
            AppUser u3 = new AppUser {Nickname = "Joe"};
            AppUser u4 = new AppUser {Nickname = "David"};
            AppUser u5 = new AppUser {Nickname = "Marco"};

            _dbContext.Add<AppUser>(u1);
            _dbContext.Add<AppUser>(u2);
            _dbContext.Add<AppUser>(u3);
            _dbContext.Add<AppUser>(u4);
            _dbContext.Add<AppUser>(u5);

            _dbContext.SaveChanges();
        }

        [Test]
        public async Task IsGettingAllFollowees()
        {
            List<Following> followees = await _repository.GetAllFolloweesFrom("1");
            Assert.AreEqual(0, followees.Count);
        }

        [Test]
        public async Task IsGettingAllFollowers()
        {
            List<Following> followers = await _repository.GetAllFollowersOf("1");
            Assert.AreEqual(0, followers.Count);
        }

        [Test]
        public async Task IsFollowingUser()
        {
            this.populate();
            AppUser follower = await _dbContext.Users.FirstOrDefaultAsync(u => u.Nickname == "Jim");
            AppUser followee = await _dbContext.Users.FirstOrDefaultAsync(u => u.Nickname == "Bob");
            List<FollowingModel> models = _dbContext.Following.ToList();
            Assert.AreEqual(0, models.Count);

            Following following = new Following {FollowerId = follower.Id, FolloweeId = followee.Id};
            following = await _repository.Create(following);
            models = _dbContext.Following.ToList();
            Assert.AreEqual(1, models.Count);
            Assert.AreNotEqual(0, following.Id);

            Assert.AreEqual(follower.Id, following.FollowerId);
            Assert.AreEqual("Jim", following.Follower.Nickname);
            Assert.AreEqual(followee.Id, following.FolloweeId);
            Assert.AreEqual("Bob", following.Followee.Nickname);
        }

        [Test]
        public async Task IsUnfollowingUser()
        {
            this.populate();
            
            AppUser follower = await _dbContext.Users.FirstOrDefaultAsync(u => u.Nickname == "Joe");
            AppUser followee = await _dbContext.Users.FirstOrDefaultAsync(u => u.Nickname == "David");

            Following following = new Following {FollowerId = follower.Id, FolloweeId = followee.Id};

            await _repository.Create(following);

            List<FollowingModel> models = _dbContext.Following.ToList();
            Assert.AreEqual(1, models.Count);

            await _repository.Remove(following);
            models = _dbContext.Following.ToList();
            Assert.AreEqual(0, models.Count);
        }

        [Test]
        public void IsThrowingWhenUnfollowingNotExist()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => {
                await _repository.Remove(new Following {FollowerId = "1", FolloweeId = "2"});
            });
        }

        [Test]
        public async Task IsCheckingExistFollowingInRepo()
        {
            FollowingModel model = new FollowingModel {FollowerId = "11", FolloweeId = "1"};
            _dbContext.Add<FollowingModel>(model);
            _dbContext.SaveChanges();

            Following f = new Following {FollowerId = "11", FolloweeId = "1"};
            bool exist = await _repository.ExistFollowing(f);
            Assert.IsTrue(exist);

            f.FolloweeId = "199";
            exist = await _repository.ExistFollowing(f);
            Assert.IsFalse(exist);
        }
    }
}