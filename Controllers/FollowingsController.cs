using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TweetishApp.Core.Services;
using TweetishApp.Core.Entities;
using TweetishApp.Core.Interfaces;

namespace TweetishApp.Controllers
{
    public class FollowingsController : Controller
    {
        // TODO: Add endpoint to check if user already follows another
        private readonly ILogger<FollowingsController> _logger;
        private readonly IFollowingService _followingService;

        public FollowingsController(ILogger<FollowingsController> logger, IFollowingService followingService)
        {
            _logger = logger;
            _followingService = followingService;
        }

        public async Task<IActionResult> Create([FromBody] Following following)
        {
            if (following.FollowerId.Length == 0 || following.FolloweeId.Length == 0) {
                return BadRequest();
            }

            try {
                await _followingService.Create(following);
            } catch (ArgumentNullException e) {
                _logger.LogDebug(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("/followings/exist/{followerId}/{followeeId}")]
        public async Task<IActionResult> ExistFollowing(string followerId, string followeeId)
        {
            bool exist = await _followingService
            .ExistFollowing(new Following { FollowerId = followerId, FolloweeId = followeeId});

            return Json(exist);
        }
    }
}