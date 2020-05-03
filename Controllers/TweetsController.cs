using Microsoft.AspNetCore.Mvc;
using System;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TweetishApp.Controllers
{
    public class TweetsController : Controller
    {
        private readonly ILogger<TweetsController> _logger;
        private readonly TweetService _tweetService;

        public TweetsController(ILogger<TweetsController> logger, TweetService tweetService)
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
    }
}