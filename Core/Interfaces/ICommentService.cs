using System.Threading.Tasks;
using System.Collections.Generic;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> Create(Comment comment);
        Task<Comment> Update(Comment comment);
        Task Remove(int commentId);
        Task<List<Comment>> GetAll(int tweetId);
    }
}