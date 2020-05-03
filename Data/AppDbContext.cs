using Microsoft.EntityFrameworkCore;
using TweetishApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TweetishApp.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FollowingModel>()
            .HasOne(fi => fi.Follower)
            .WithMany(fr => fr.Followings)
            .HasForeignKey(fi => fi.FollowerId);

            builder.Entity<FollowingModel>()
            .HasOne(fi => fi.Followee)
            .WithMany(fe => fe.Followings)
            .HasForeignKey(fi => fi.FolloweeId);
        }

        public DbSet<TweetModel> Tweet {get; set;}
    }
}