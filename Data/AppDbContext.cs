using Microsoft.EntityFrameworkCore;

namespace TweetishApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}
    }
}