using TweetishApp.Core.Interfaces;
using System.Threading.Tasks;
using TweetishApp.Core.Entities;
using System.Collections.Generic;

namespace TweetishApp.Core.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _repository;

        public ReactionService(IReactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Reaction> Create(Reaction reaction)
        {
            return await _repository.Create(reaction);
        }

        public async Task<List<Reaction>> GetAll()
        {
            return await _repository.GetAll();
        }
    }
}