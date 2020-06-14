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
            .WithMany()
            .HasForeignKey(fi => fi.FollowerId);

            builder.Entity<FollowingModel>()
            .HasOne(fi => fi.Followee)
            .WithMany()
            .HasForeignKey(fi => fi.FolloweeId);

            builder.Entity<RetweetModel>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.Retweets)
            .HasForeignKey(rt => rt.UserId);

            builder.Entity<RetweetModel>()
            .HasOne(rt => rt.Tweet)
            .WithMany(t => t.Retweets)
            .HasForeignKey(rt => rt.TweetId);

            builder.Entity<UserTweetReactionModel>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reactions)
            .HasForeignKey(r => r.UserId);

            // Constraints
            builder.Entity<FollowingModel>()
            .HasIndex(f => new {f.FollowerId, f.FolloweeId})
            .IsUnique();
        }

        public DbSet<TweetModel> Tweet {get; set;}
        public DbSet<FollowingModel> Following {get; set;}
        public DbSet<RetweetModel> Retweet {get; set;}
        public DbSet<ReactionModel> Reaction {get; set;}
        public DbSet<UserTweetReactionModel> UserTweetReaction {get; set;}
        public DbSet<CommentModel> Comment {get; set;}
    }
}