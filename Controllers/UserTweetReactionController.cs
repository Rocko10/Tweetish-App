using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;

namespace TweetishApp.Controllers
{
    public class UserTweetReactionController : Controller
    {
        private readonly ILogger<UserTweetReactionController> _logger;
        private readonly IUserTweetReactionService _userTweetReactionService;

        public UserTweetReactionController(
            ILogger<UserTweetReactionController> logger,
            IUserTweetReactionService userTweetReactionService
        )
        {
            _logger = logger;
            _userTweetReactionService = userTweetReactionService;
        }

        [HttpPost]
        public async Task<IActionResult> Toggle([FromBody] UserTweetReaction userTweetReaction)
        {
            try {
                await _userTweetReactionService.Toggle(userTweetReaction);
            } catch (ArgumentNullException e) {
                _logger.LogError(e.Message);
            }

            return Ok();
        }
    }
}