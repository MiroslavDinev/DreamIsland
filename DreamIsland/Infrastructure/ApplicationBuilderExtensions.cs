namespace DreamIsland.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using DreamIsland.Data;
    using DreamIsland.Data.Models.Islands;

    using static WebConstants;
    using System.Linq;
    using DreamIsland.Data.Models;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;

            var data = serviceProvider.GetRequiredService<DreamIslandDbContext>();

            data.Database.Migrate();

            SeedPopulationSize(data);
            SeedRegions(data);
            SeedAdministrator(serviceProvider);

            return app;
        }

        private static void SeedRegions(DreamIslandDbContext data)
        {
            if (data.IslandRegions.Any())
            {
                return;
            }

            data.IslandRegions.AddRange(new[]
            {
                new IslandRegion {Name = "Africa"},
                new IslandRegion {Name = "Asia"},
                new IslandRegion {Name = "Canada"},
                new IslandRegion {Name = "Carribean"},
                new IslandRegion {Name = "Central America"},
                new IslandRegion {Name = "Europe"},
                new IslandRegion {Name = "South America"},
                new IslandRegion {Name = "South Pacific"},
                new IslandRegion {Name = "United States"}
            });

            data.SaveChanges();
        }

        private static void SeedPopulationSize(DreamIslandDbContext data)
        {
            if (data.PopulationSizes.Any())
            {
                return;
            }

            data.PopulationSizes.AddRange( new []
            {
                new PopulationSize {Name = "Uninhabited"},
                new PopulationSize {Name = "Up to 10 persons"},
                new PopulationSize {Name = "Between 10 persons and 100 persons"},
                new PopulationSize {Name = "Between 100 persons and 1000 persons"},
                new PopulationSize {Name = "Between 1000 persons and 10000 persons"},
                new PopulationSize {Name = "Between 10000 persons and 100000 persons"},
                new PopulationSize {Name = "Between 100000 persons and 1000000 persons"},
                new PopulationSize {Name = "Over 1000000"}
            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
               {
                   if(await roleManager.RoleExistsAsync(AdministratorRoleName))
                   {
                       return;
                   }

                   var role = new IdentityRole { Name = AdministratorRoleName };
                   await roleManager.CreateAsync(role);

                   const string adminEmail = "admin.dreamisland@dir.bg";
                   const string adminPassword = "admin123!321!";

                   var user = new User
                   {
                       Email = adminEmail,
                       UserName = adminEmail,
                       Nickname = "Admin"
                   };

                   await userManager.CreateAsync(user, adminPassword);
                   await userManager.AddToRoleAsync(user, role.Name);

               })
                .GetAwaiter()
                .GetResult();
        }
    }
}
