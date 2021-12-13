namespace DreamIsland.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using DreamIsland.Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scoperServices = app.ApplicationServices.CreateScope();

            var data = scoperServices.ServiceProvider.GetService<DreamIslandDbContext>();

            data.Database.Migrate();

            return app;
        }
    }
}
