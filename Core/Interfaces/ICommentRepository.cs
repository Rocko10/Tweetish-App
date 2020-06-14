using TweetishApp.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TweetishApp.Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> Create(Comment comment);
        Task<Comment> Update(Comment comment);
        Task Remove(int commentId);
        Task<List<Comment>> GetAll(int tweetId);
    }
}