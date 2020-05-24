using Microsoft.AspNetCore.Mvc;
using TweetishApp.Models;
using System.Collections.Generic;
using TweetishApp.Core.Services;
using TweetishApp.Core.Interfaces;
using TweetishApp.Core.Entities;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TweetishApp.Data;
using Microsoft.Extensions.Logging;

namespace TweetishApp.Controllers
{
    public class UsersController : ApplicationController
    {
        private readonly ILogger<UsersController> _logger;
        private UserManager<AppUser> _userManager;
        private ITweetService _tweetService;

        public UsersController(ILogger<UsersController> logger, UserManager<AppUser> userManager, ITweetService tweetService)
        {
            _logger = logger;
            _userManager = userManager;
            _tweetService = tweetService;
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View("Dashboard", userId);
        }

        [Route("/users/show/{userId}")]
        public async Task<IActionResult> Show(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null) {
                _logger.LogDebug("user not found");

                return NotFound();
            }

            return View(new UserProfileVM(user.Id, user.Nickname));
        }
    }
}