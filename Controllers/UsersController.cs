using Microsoft.AspNetCore.Mvc;
using TweetishApp.Models;
using System.Collections.Generic;
using System;
using TweetishApp.Core.Services;
using TweetishApp.Core.Interfaces;
using TweetishApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        [Route("/users/options/{nicknameGuess}")]
        public  IActionResult GetNicknameOptions(string nicknameGuess)
        {
            IQueryable<Tuple<string, string>> query = from u in _userManager.Users
            where EF.Functions.Like(u.Nickname, $"%{nicknameGuess}%")
            select new Tuple<string, string>(u.Nickname, u.Id);

            List<Tuple<string, string>> options = query.ToList();

            return Json(options);
        }
    }
}