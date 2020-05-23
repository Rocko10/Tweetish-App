using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace TweetishApp.Controllers
{
    public class UsersController : ApplicationController
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View("Dashboard", userId);
        }
    }
}