using Microsoft.EntityFrameworkCore;
using TweetishApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TweetishApp.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Tweet> Tweet {get; set;}
    }
}