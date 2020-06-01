using TweetishApp.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TweetishApp.Core.Interfaces
{
    public interface IReactionService
    {
        Task<Reaction> Create(Reaction reaction);
        Task<List<Reaction>> GetAll();
    }
}