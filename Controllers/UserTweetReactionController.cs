using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet]
        [Route("/userTweetReaction/reacted/{userId}/{tweetId}/{reactionId}")]
        public async Task<IActionResult> Reacted(string userId, int tweetId, int reactionId)
        {
            UserTweetReaction userTweetReaction = new UserTweetReaction
            { UserId = userId, TweetId = tweetId, ReactionId = reactionId };

            userTweetReaction = await _userTweetReactionService.Reacted(userTweetReaction);

            return Ok(userTweetReaction);
        }

        [HttpPost]
        public async Task<IActionResult> ReactedToMany([FromBody] IEnumerable<UserTweetReaction> reactions)
        {
            await _userTweetReactionService.ReactedToMany(reactions);

            return Ok(reactions);
        }
    }
}