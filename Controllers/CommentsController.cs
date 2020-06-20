using Microsoft.AspNetCore.Mvc;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TweetishApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ILogger<CommentsController> _logger;

        private readonly ICommentService _commentService;

        public CommentsController(ILogger<CommentsController> logger, ICommentService commentService)
        {
            _logger = logger;
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<Comment> Create([FromBody] Comment comment)
        {
            return await _commentService.Create(comment); 
        }
    }
}