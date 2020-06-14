using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TweetishApp.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using TweetishApp.Models;

namespace TweetishApp.Data
{

    [TestFixture]
    public class CommentRepositoryTest
    {
        private AppDbContext _dbContext;
        private CommentRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: "comment_db_test")
            .Options;

            _dbContext = new AppDbContext(options);
            _repository = new CommentRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            List<AppUser> users = _dbContext.Users.ToList();
            List<TweetModel> tweets = _dbContext.Tweet.ToList();
            List<CommentModel> comments = _dbContext.Comment.ToList();

            foreach (AppUser u in users) {
                _dbContext.Remove<AppUser>(u);
            }

            foreach (TweetModel t in tweets) {
                _dbContext.Remove<TweetModel>(t);
            }

            foreach (CommentModel c in comments) {
                _dbContext.Remove<CommentModel>(c);
            }

            _dbContext.SaveChanges();
        }

        public (AppUser, TweetModel) populate()
        {
            AppUser user = new AppUser {Nickname = "marcow"};
            _dbContext.Add<AppUser>(user);
            _dbContext.SaveChanges();

            TweetModel tweet = new TweetModel {UserId = user.Id, Text = "First text"};
            _dbContext.Add<TweetModel>(tweet);
            _dbContext.SaveChanges();

            return (user, tweet);
        }

        [Test]
        public async Task IsCreatingCommentInRepo()
        {
            (AppUser, TweetModel) items = this.populate();
            Comment c1 = new Comment {TweetId = items.Item2.Id, Text = "Cool"};
            Comment c2 = new Comment {TweetId = items.Item2.Id, Text = "Very Cool"};
            await _repository.Create(c1);
            await _repository.Create(c2);

            List<Comment> comments = await _repository.GetAll(items.Item2.Id);
            Assert.AreEqual(2, comments.Count);
            Assert.AreEqual("First text", comments[0].Tweet.Text);
            Assert.AreEqual("First text", comments[1].Tweet.Text);
            Assert.AreEqual("marcow", comments[0].Tweet.Nickname);
        }

        [Test]
        public async Task IsUpdatingCommentInRepo()
        {
            (AppUser, TweetModel) items = this.populate();
            Comment c1 = new Comment {TweetId = items.Item2.Id, Text = "Yeah"};
            await _repository.Create(c1);

            List<Comment> comments = await _repository.GetAll(items.Item2.Id);
            Assert.AreEqual(1, comments.Count);
            Assert.AreEqual("Yeah", comments[0].Text);

            c1.Text = "Not Yeah";
            Task<Comment> t1 = _repository.Update(c1);

            comments = await _repository.GetAll(items.Item2.Id);

            c1 = await t1;
            Assert.AreEqual("Not Yeah", comments[0].Text);
        }

        [Test]
        public async Task IsRemovingCommentInRepo()
        {
            (AppUser, TweetModel) items = this.populate();
            Comment comment = new Comment {TweetId = items.Item2.Id, Text = "Super"};
            comment = await _repository.Create(comment);

            List<Comment> comments = await _repository.GetAll(items.Item2.Id);
            Assert.AreEqual(1, comments.Count);

            await _repository.Remove(comment.Id);
            comments = await _repository.GetAll(items.Item2.Id);
            Assert.AreEqual(0, comments.Count);
        }

        [Test]
        public void IsThrowingWithInvalidCommentId()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => {
                await _repository.Remove(11);
            });
        }

    }
}