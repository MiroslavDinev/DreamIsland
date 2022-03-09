namespace DreamIsland
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity.UI.Services;


    using DreamIsland.Data;
    using DreamIsland.Infrastructure;
    using DreamIsland.Services.Car;
    using DreamIsland.Services.Partner;
    using DreamIsland.Data.Models;
    using DreamIsland.Services.Island;
    using DreamIsland.Services.Statistic;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Services.Collectible;
    using DreamIsland.Services.Messaging;
    using DreamIsland.Hubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DreamIslandDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<User>(options => 
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedAccount = true;

            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DreamIslandDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddMemoryCache();

            services.AddControllersWithViews(options=> 
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddSignalR();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IPartnerService, PartnerService>();
            services.AddTransient<IIslandService, IslandService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<ICelebrityService, CelebrityService>();
            services.AddTransient<ICollectibleService, CollectibleService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultAreaRoute();

                endpoints.MapControllerRoute(
                    name: "Island Details",
                    pattern: "/Islands/Details/{id}/{information}",
                    defaults: new { controller = "Islands", action = "Details" });

                endpoints.MapControllerRoute(
                    name: "Car Details",
                    pattern: "/Cars/Details/{id}/{information}",
                    defaults: new { controller = "Cars", action = "Details" }
                    );

                endpoints.MapControllerRoute(
                    name: "Celebrity Details",
                    pattern: "/Celebrities/Details/{id}/{information}",
                    defaults: new { controller = "Celebrities", action = "Details" }
                    );

                endpoints.MapControllerRoute(
                    name: "Collectible Details",
                    pattern: "/Collectibles/Details/{id}/{information}",
                    defaults: new {controller= "Collectibles", action= "Details" }
                    );

                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();

                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
