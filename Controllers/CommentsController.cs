using Microsoft.AspNetCore.Mvc;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace TweetishApp.Controllers
{
    public class CommentsController
    {
        private readonly ILogger<CommentsController> _logger;

        private readonly ICommentService _commentService;

        public CommentsController(ILogger<CommentsController> logger, ICommentService commentService)
        {
            _logger = logger;
            _commentService = commentService;
        }

        public void Create([FromBody] Comment comment)
        {
            _logger.LogInformation($"Comment id: {comment.Id}");
            _logger.LogInformation($"Comment tweetId: {comment.TweetId}");
            _logger.LogInformation($"Comment text: {comment.Text}");
        }
    }
}