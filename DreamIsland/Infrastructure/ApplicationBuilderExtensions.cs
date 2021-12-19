namespace DreamIsland.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using DreamIsland.Data;
    using DreamIsland.Data.Models;

    using static WebConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;

            var data = serviceProvider.GetRequiredService<DreamIslandDbContext>();

            data.Database.Migrate();

            SeedAdministrator(serviceProvider);

            return app;
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
