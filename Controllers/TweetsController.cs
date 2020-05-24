using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TweetishApp.Controllers
{
    public class TweetsController : Controller
    {
        private readonly ILogger<TweetsController> _logger;
        private readonly ITweetService _tweetService;

        public TweetsController(ILogger<TweetsController> logger, ITweetService tweetService)
        {
            _logger = logger;
            _tweetService = tweetService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tweet tweet)
        {
            try {
                await _tweetService.Create(tweet);
            } catch(ArgumentException e) {
                _logger.LogError(e.Message);

                return BadRequest();
            }

            return Ok();            
        }

        [HttpGet]
        [Route("/tweets/getTweetsBy/{userId}")]
        public async Task<IActionResult> GetTweetsBy(string userId)
        {
            List<Tweet> tweets = await _tweetService.GetTweetsBy(userId);

            return Json(tweets);
        }
    }
}