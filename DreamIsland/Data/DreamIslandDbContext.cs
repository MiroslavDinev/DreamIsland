namespace DreamIsland.Data
{
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using DreamIsland.Data.Models;
    using DreamIsland.Data.Models.Vehicles;
    using DreamIsland.Data.Models.Islands;
    using DreamIsland.Data.Models.Celebrities;

    public class DreamIslandDbContext : IdentityDbContext<User>
    {
        public DreamIslandDbContext(DbContextOptions<DreamIslandDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Island> Islands { get; set; }
        public DbSet<Celebrity> Celebrities { get; set; }
        public DbSet<CelebrityGallery> CelebritiesGalleries { get; set; }
        public DbSet<Collectible> Collectibles { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PopulationSize> PopulationSizes { get; set; }
        public DbSet<IslandRegion> IslandRegions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
