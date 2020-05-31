using Microsoft.AspNetCore.Mvc;
using System;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TweetishApp.Controllers
{
    public class RetweetsController : Controller
    {
        private readonly ILogger<RetweetsController> _logger;
        private readonly IRetweetService _retweetService;

        public RetweetsController(ILogger<RetweetsController> logger, IRetweetService retweetService)
        {
            _logger = logger;
            _retweetService = retweetService;
        }

        [HttpGet]
        [Route("/retweets/toggle/{profileId}/{tweetId}")]
        public async Task<IActionResult> Toggle(string profileId, int tweetId)
        {
            Retweet retweet = new Retweet {UserId = profileId, TweetId = tweetId};

            try {
                await _retweetService.Toggle(retweet); 
            } catch (ArgumentNullException e) { 
                _logger.LogInformation(e.Message);

                return BadRequest();
            }

            return Ok();
        }
    }
}