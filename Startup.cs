using System;
using TweetishApp.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TweetishApp.Data;
using Microsoft.AspNetCore.Hosting;
using TweetishApp.Core.Interfaces;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TweetishApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlite(Configuration.GetConnectionString("Default"));
            });
            services.AddIdentity<AppUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureExternalCookie(options => {
                options.LoginPath = "/accounts/login";
                options.LogoutPath = "/accounts/logout";
            });

            services.AddScoped<ITweetRepository, TweetRepository>();
            services.AddScoped<ITweetService, TweetService>();
            services.AddScoped<IRetweetRepository, RetweetRepository>();
            services.AddScoped<IRetweetService, RetweetService>();
            services.AddScoped<IFollowingRepository, FollowingRepository>();
            services.AddScoped<IFollowingService, FollowingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
