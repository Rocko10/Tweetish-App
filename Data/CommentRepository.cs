using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using TweetishApp.Models;
using System;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;

namespace TweetishApp.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _dbContext;

        public CommentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Comment>> GetAll(int tweetId)
        {
            List<Comment> comments = new List<Comment>();
            List<CommentModel> models = new List<CommentModel>();

            models = await _dbContext.Comment
            .Include(c => c.Tweet)
            .ThenInclude(t => t.User)
            .Where(c => c.TweetId == tweetId)
            .ToListAsync();

            foreach (CommentModel c in models) {
                Tweet tweet = new Tweet
                {
                    Id = c.Tweet.Id,
                    Text = c.Tweet.Text,
                    UserId = c.Tweet.UserId,
                    Nickname = c.Tweet.User.Nickname
                };

                Comment comment = new Comment
                {
                    Id = c.Id,
                    Text = c.Text,
                    TweetId = c.TweetId,
                    Tweet = tweet
                };

                comments.Add(comment);
            }

            return comments;
        }

        public async Task<Comment> Create(Comment comment)
        {
            TweetModel tweetModel = await _dbContext.Tweet.FirstOrDefaultAsync(t => t.Id == comment.TweetId);

            if (tweetModel == null) {
                throw new ArgumentNullException("Tweet not found");
            }

            CommentModel model = new CommentModel
            {
                TweetId = comment.TweetId,
                Text = comment.Text
            };

            _dbContext.Add<CommentModel>(model);
            await _dbContext.SaveChangesAsync();

            comment.Id = model.Id;

            return comment;
        }

        public async Task<Comment> Update(Comment comment)
        {
            CommentModel model = await _dbContext.Comment.FirstOrDefaultAsync(c => c.Id == comment.Id);

            if (model == null) {
                throw new ArgumentException($"Comment with {comment.Id} not found");
            }

            model.Text = comment.Text;

            _dbContext.Update<CommentModel>(model);
            await _dbContext.SaveChangesAsync();

            return comment;
        }
        
        public async Task Remove(int commentId)
        {
            CommentModel model = await _dbContext.Comment.FirstOrDefaultAsync(c => c.Id == commentId);

            if (model == null) {
                throw new ArgumentException($"Comment with commentId: {commentId} not found");
            }

            _dbContext.Remove<CommentModel>(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}