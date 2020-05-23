using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TweetishApp.Controllers
{
    public abstract class ApplicationController : Controller
    {
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            ViewData["userId"] = null;

            if (User.Identity.IsAuthenticated) {
                ViewData["userId"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }
    }
}