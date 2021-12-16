namespace DreamIsland.Data
{
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using DreamIsland.Data.Models;
    using DreamIsland.Data.Models.Vehicles;

    public class DreamIslandDbContext : IdentityDbContext
    {
        public DreamIslandDbContext(DbContextOptions<DreamIslandDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Island> Islands { get; set; }
        public DbSet<Celebrity> Celebrities { get; set; }
        public DbSet<Collectible> Collectibles { get; set; }
        public DbSet<Partner> Partners { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
