using TweetishApp.Core.Interfaces;
using System.Collections.Generic;
using TweetishApp.Core.Entities;
using System.Threading.Tasks;

namespace TweetishApp.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;

        public CommentService(ICommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Comment> Create(Comment comment)
        {
            return await _repository.Create(comment);
        }

        public async Task<Comment> Update(Comment comment)
        {
            return await _repository.Update(comment);
        }

        public async Task Remove(int tweetId)
        {
            await _repository.Remove(tweetId);
        }

        public async Task<List<Comment>> GetAll(int tweetId)
        {
            return await _repository.GetAll(tweetId);
        }
    }
}