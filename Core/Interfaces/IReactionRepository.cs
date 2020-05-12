using TweetishApp.Core.Entities;
using System.Threading.Tasks;

namespace TweetishApp.Core.Interfaces
{
    public interface IReactionRepository
    {
        Task<Reaction> Create(Reaction reaction);
    }
}