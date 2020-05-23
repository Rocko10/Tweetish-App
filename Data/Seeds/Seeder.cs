using TweetishApp.Data;
using TweetishApp.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TweetishApp.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TweetishApp.Data.Seeds
{
    public static class Seeder
    {
        public static void Up()
        {
            Console.WriteLine("Seeding...");
            var options = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=app.db")
            .Options;
            AppDbContext dbContext = new AppDbContext(options);

            AppUser u1 = new AppUser {Nickname = "Joe" , UserName = "Joe", NormalizedUserName = "JOE", PasswordHash = "AQAAAAEAACcQAAAAEA6OCQQajV1pU1Yn8ockUkVekZo63ozhhE0nCndh5O3NLeikSeuJapJo1f4NyGFu5A=="};
            AppUser u2 = new AppUser {Nickname = "Mike", UserName = "Mike", NormalizedUserName = "MIKE",PasswordHash = "AQAAAAEAACcQAAAAEA6OCQQajV1pU1Yn8ockUkVekZo63ozhhE0nCndh5O3NLeikSeuJapJo1f4NyGFu5A=="};
            AppUser u3 = new AppUser {Nickname = "Tim", UserName = "Tim" , NormalizedUserName = "TIM", PasswordHash = "AQAAAAEAACcQAAAAEA6OCQQajV1pU1Yn8ockUkVekZo63ozhhE0nCndh5O3NLeikSeuJapJo1f4NyGFu5A=="};
            dbContext.Add<AppUser>(u1);
            dbContext.Add<AppUser>(u2);
            dbContext.Add<AppUser>(u3);
            dbContext.SaveChanges();


            TweetModel t1 = new TweetModel {UserId = u1.Id, Text = "Robots are cool!"};
            TweetModel t2 = new TweetModel {UserId = u1.Id, Text = "Like turtles!"};
            TweetModel t3 = new TweetModel {UserId = u1.Id, Text = "Going to run"};
            TweetModel t4 = new TweetModel {UserId = u2.Id, Text = "Watching videos"};
            TweetModel t5 = new TweetModel {UserId = u2.Id, Text = "Doing laundry"};
            TweetModel t6 = new TweetModel {UserId = u3.Id, Text = "Making dinner"};

            dbContext.Add<TweetModel>(t1);
            dbContext.Add<TweetModel>(t2);
            dbContext.Add<TweetModel>(t3);
            dbContext.Add<TweetModel>(t4);
            dbContext.Add<TweetModel>(t5);
            dbContext.Add<TweetModel>(t6);
            dbContext.SaveChanges();
            Console.WriteLine("Finished seeding");
        }

        public static void Down()
        {
            Console.WriteLine("Unseeding...");
            var options = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=app.db")
            .Options;
            AppDbContext dbContext = new AppDbContext(options);

            List<TweetModel> tweets = dbContext.Tweet.ToList();
            foreach (TweetModel t in tweets) {
                dbContext.Remove<TweetModel>(t);
            }
            dbContext.SaveChanges();

            List<AppUser> users = dbContext.Users.ToList();
            foreach (AppUser u in users) {
                dbContext.Remove<AppUser>(u);
            }
            dbContext.SaveChanges();
            Console.WriteLine("Finished");
        }
    }
}