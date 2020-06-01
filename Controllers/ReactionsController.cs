using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TweetishApp.Core.Interfaces;
using TweetishApp.Core.Services;
using TweetishApp.Core.Entities;

namespace TweetishApp.Controllers
{
    public class ReactionsController : Controller
    {
        private readonly ILogger<ReactionsController> _logger;
        private readonly IReactionService _reactionService;

        public ReactionsController(ILogger<ReactionsController> logger, IReactionService reactionService)
        {
            _logger = logger;
            _reactionService = reactionService;
        }

        public async Task<IActionResult> GetAll()
        {
            List<Reaction> reactions = await _reactionService.GetAll();

            return Ok(reactions);
        }
    }
}