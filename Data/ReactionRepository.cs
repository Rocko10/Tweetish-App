using TweetishApp.Core.Interfaces;
using TweetishApp.Models;
using TweetishApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TweetishApp.Data
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly AppDbContext _dbContext;

        public ReactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Reaction> Create(Reaction reaction)
        {
            ReactionModel model = new ReactionModel { Name = reaction.Name };

            _dbContext.Add<ReactionModel>(model);
            await _dbContext.SaveChangesAsync();

            reaction.Id = model.Id;

            return reaction;
        }
    }
}