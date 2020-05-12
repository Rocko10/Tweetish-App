using NUnit.Framework;
using TweetishApp.Core.Entities;
using TweetishApp.Models;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace TweetishApp.Data
{
    [TestFixture]
    public class ReactionRepositoryTest
    {
        private AppDbContext _dbContext;
        private ReactionRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: "reaction_test")
            .Options;

            _dbContext = new AppDbContext(options);
            _repository = new ReactionRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            List<ReactionModel> models = _dbContext.Reaction.ToList();

            foreach (ReactionModel m in models) {
                _dbContext.Remove<ReactionModel>(m);
            }

            _dbContext.SaveChanges();
        }

        [Test]
        public async Task IsCreatingReactionInRepo()
        {
            Reaction reaction = new Reaction("Happy"); 
            Assert.AreEqual(0, reaction.Id);

            reaction = await _repository.Create(reaction);
            Assert.AreEqual(1, reaction.Id);
        }
    }
}